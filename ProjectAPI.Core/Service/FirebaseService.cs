using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace ProjectAPI.Core.Service
{
    public static class FirebaseService
    {
        private static string _apiKey = "";
        private static string _idToken = "";
        private static string _jsonData = "";
        private static string _apiUrl;
        private static StringContent _postData; 
        public static string GetIp()
        {
            string hostname = Dns.GetHostName();
            IPHostEntry ipentry = Dns.GetHostEntry(hostname);
            IPAddress[] addr = ipentry.AddressList;
            return addr[addr.Length - 2].ToString();
        }
        public static async void Authenticate()
        {
            var user = new User();
            using (StreamReader file = File.OpenText("secret.json"))
            {
                dynamic credentials = JsonConvert.DeserializeObject(file.ReadToEnd()) ;
                user.email = credentials["email"];
                user.password = credentials["password"];
                _apiKey = credentials["key"];
                _apiUrl = credentials["apiurl"];
            }
            user.returnSecureToken = true;
            _jsonData = JsonConvert.SerializeObject(user);
            var postData = new StringContent(_jsonData, Encoding.UTF8, "application/json");
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}";
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url, postData);
                var result = response.Result.Content.ReadAsStringAsync().Result;
                var temporaryResult = result.Split(",")[4].Split(": ")[1];
                _idToken = temporaryResult.Substring(1, temporaryResult.Length - 2);
            }
            if (_idToken != "") await PostIp();
        }
        public static async Task PostIp()
        {
            using (var client = new HttpClient())
            {
                var localEndPoint = GetIp() + ":5000";
                dynamic updatedIp = new ExpandoObject();
                updatedIp.ip = localEndPoint;
                _jsonData = JsonConvert.SerializeObject(updatedIp);
                _postData = new StringContent(_jsonData, Encoding.UTF8, "application/json");
                await client.DeleteAsync($"{_apiUrl}?auth={_idToken}");
                await client.PostAsync($"{_apiUrl}?auth={_idToken}", _postData);
            }
        }
    }
}