using HotelAPI.HotelAPI.Core.Exceptions;
using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
namespace HotelAPI.Core.Service
{
    public static class FirebaseService
    {
        private static string _apiKey = "";
        private static string _idToken = "";
        private static string _jsonData = "";
        private static string _firebaseUrl;
        private static StringContent _postData;
        private static int _port;
        private static string _firebaseAuthUrl;
        public static string GetIp()
        {
            //var hostname = Dns.GetHostName();
            //var ipentry = Dns.GetHostEntry(hostname);
            //var addr = ipentry.AddressList;
            //return addr?[addr.Length - 3].ToString();
            var networkAdapters = NetworkInterface.GetAllNetworkInterfaces();
            string ipAddress = "";
            bool wifiIPFound = false;
            foreach(var networkAdapter in networkAdapters)
            {
                if (wifiIPFound)
                    break;
                if(networkAdapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    var ipProperties = networkAdapter.GetIPProperties();
                    foreach(var unicastAddress in ipProperties.UnicastAddresses)
                    {
                        if(!IPAddress.IsLoopback(unicastAddress.Address) &&
                            unicastAddress.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            if (networkAdapter.Name.Equals("Wi-Fi"))
                            {
                                ipAddress = unicastAddress.Address.ToString();
                                wifiIPFound = true;
                            }
                        }
                    }
                }
            }
            return ipAddress;
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
                _firebaseUrl = credentials["firebaseurl"];
                _port = credentials["port"];
                _firebaseAuthUrl = credentials["firebaseauthurl"];
            }
            user.returnSecureToken = true;
            _jsonData = JsonConvert.SerializeObject(user);
            var postData = new StringContent(_jsonData, Encoding.UTF8, "application/json");
            var url = $"{_firebaseAuthUrl}?key={_apiKey}";
            using (var client = new HttpClient())   
            {
                ExtractIdToken(client, url, postData);
            }
            try
            {
                if (_idToken != "") await PostIp();
            }
            catch (InvalidHostException)
            {
                throw;
            }
        }
        private static void ExtractIdToken(HttpClient client, string url, StringContent postData)
        {
            var response = client.PostAsync(url, postData);
            var result = response.Result.Content.ReadAsStringAsync().Result;
            var temporaryResult = result.Split(",")[4].Split(": ")[1];
            _idToken = temporaryResult.Substring(1, temporaryResult.Length - 2);
        }
        public static async Task PostIp()
        {
            using (var client = new HttpClient())
            {
                var ipAddress = GetIp();
                if (ipAddress == null) throw new InvalidHostException();
                var localEndPoint = GetIp() + $":{_port}";
                dynamic updatedIp = new ExpandoObject();
                updatedIp.ip = localEndPoint;
                _jsonData = JsonConvert.SerializeObject(updatedIp);
                _postData = new StringContent(_jsonData, Encoding.UTF8, "application/json");
                await client.DeleteAsync($"{_firebaseUrl}?auth={_idToken}");
                await client.PostAsync($"{_firebaseUrl}?auth={_idToken}", _postData);
            }
        }
    }
}