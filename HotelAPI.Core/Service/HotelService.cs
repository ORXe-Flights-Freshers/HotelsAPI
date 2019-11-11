using HotelAPI.HotelAPI.Core.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI.HotelAPI.Core.Service
{
    public class HotelService
    {
        public RestClient GetRestClient()
        {
            RestClient client = null;
            try
            {
               client = new RestClient(GetHotelApiUrl());
            }
            catch (ApiUrlException)
            {
                Console.WriteLine("");
            }
            return client;
        }

        private string GetHotelApiUrl()
        {
            string hotelApiUrl = "";
            using (StreamReader file = File.OpenText("secret.json"))
            {
                dynamic credentials = JsonConvert.DeserializeObject(file.ReadToEnd());
                hotelApiUrl =  credentials["apiurl"];
            }
            return (hotelApiUrl != null) ? hotelApiUrl : throw new ApiUrlException();
        }
    }
}
