using HotelAPI.Configuration;
using HotelAPI.HotelAPI.Core.Exceptions;
using Microsoft.Extensions.Logging;
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
        private string _fileName;
        private string _apiUrl = "";
        private readonly ILogger<HotelService> _logger;
        public HotelService(AppSettings appSettings, ILogger<HotelService> logger)
        {
            _fileName = appSettings.ConfigurationFileName;
            _logger = logger;
        }
        public RestClient GetRestClient()
        {
            RestClient client = null;
            try
            {
               client = new RestClient(GetHotelApiUrl());
            }
            catch (ApiUrlException aue)
            {
                _logger.LogError(aue.ToString());
            }
            catch (FileNotFoundException fne)
            {
                _logger.LogError(fne.ToString());
            }
            return client;
        }

        private string GetHotelApiUrl()
        {
            string hotelApiUrl = "";
            using (StreamReader file = File.OpenText(_fileName))
            {
                if (file == null) throw new FileNotFoundException($"File {_fileName} not found");
                dynamic credentials = JsonConvert.DeserializeObject(file.ReadToEnd());
                hotelApiUrl =   (_apiUrl != "") ? _apiUrl : credentials["apiurl"];
            }
            return (hotelApiUrl != null) ? hotelApiUrl : throw new ApiUrlException();
        }
    }
}
