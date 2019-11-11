using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HotelAPI.Cache;
using HotelAPI.Exceptions;
using HotelAPI.HotelAPI.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RestSharp;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private IDistributedCache _distributedCache;
        private HotelService _hotelService;
        public HotelsController(IDistributedCache distributedCache, HotelService hotelService)
        {
            _distributedCache = distributedCache;
            _hotelService = hotelService;
        }
        private static readonly HttpClient client = new HttpClient();

        [HttpGet("{lattitude}/{longitude}")]
        public async Task<ActionResult<string>> GetAsync(string lattitude,string longitude)
        {
            string dataFromCache = "";
            dataFromCache = await Redis.GetObjectAsync(_distributedCache, lattitude + "," + longitude);
            if (dataFromCache != null) return dataFromCache;
            var response = GetResponse(lattitude, longitude);
            await Redis.SetObjectAsync(_distributedCache, lattitude + "," + longitude, response);
            dataFromCache = response;
            return dataFromCache;
        }
        private string GetResponse(string lattitude, string longitude)
        {
            string response = "";
            RestClient restClient = null;
            try
            {
                restClient = GetRestClient();
            }
            catch (RestClientNotFoundException)
            {
                response = null;
            }
            response = ProcessRequest(restClient, lattitude, longitude);
            return response;
        }

        private string ProcessRequest(RestClient restClient, string lattitude, string longitude)
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Content-Length", "228");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "hotel-loyaltycontent.stage.cnxloyalty.com");
            request.AddHeader("Postman-Token", "aeecd0c2-ecfd-4ba6-b4db-435e480c1d98,130e6411-3ffc-408b-a8cf-c2ea0fb57afd");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.18.0");
            request.AddHeader("oski-tenantId", "demo");
            request.AddHeader("oski-correlationId", "2a04a6f-593f-4de4-25fc-jkh");
            request.AddHeader("oski-clientTenantId", "demo");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{  \n    \"georegion\": {\n    \"circle\": {\n      \"center\": {\n        \"lat\":  " + lattitude + ",\n        \"long\":  " + longitude + "\n      },\n      \"radiusKm\": 2\n    }\n  },\n  \"supplierFamilies\": [\n    \"ean\"\n  ],\n  \"contentPrefs\": [\n    \"Basic\"\n  ]\n}", ParameterType.RequestBody);
            var response = restClient.Execute(request);
            return response.Content;
        }

        private RestClient GetRestClient()
        {
            var client = _hotelService.GetRestClient();
            return (client != null) ? client : throw new RestClientNotFoundException();
        }
    }
}
