using HotelAPI.Cache;
using HotelAPI.HotelAPI.Core.Exceptions;
using HotelAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelAPI.HotelAPI.Core.Service
{
    public class HotelResponse
    {
        private ILogger<HotelResponse> _logger;

        private IDistributedCache _distributedCache;

        private HotelService _hotelService;

        public HotelResponse(HotelService hotelService, IDistributedCache distributedCache
                                , ILogger<HotelResponse> logger)
        {
            _hotelService = hotelService;
            _distributedCache = distributedCache;
            _logger = logger;
        }
        private static readonly HttpClient client = new HttpClient();
        public async Task<ActionResult<HotelList>> Get(string lattitude, string longitude, int radius)
        {

            string dataFromCache = "";
            var t = new HotelList();
            if (_distributedCache != null)
                dataFromCache = await Redis.GetObjectAsync(_distributedCache, lattitude + "," + longitude + "," + radius);

            if (dataFromCache != null)
            {
                var result = JsonConvert.DeserializeObject<HotelList>(dataFromCache);
                return result;
            }
            var response = new HotelList();
            try
            {
                response = GetResponse(lattitude, longitude, radius);
            }
            catch (InvalidPlaceException ipe)
            {
                _logger.LogError(ipe.Message);
            }
            if (_distributedCache != null)
                await Redis.SetObjectAsync(_distributedCache, lattitude + "," + longitude + "," + radius, JsonConvert.SerializeObject(response));
            return response;
        }
        private HotelList GetResponse(string lattitude, string longitude, int radius)
        {
            var response = new HotelList();
            RestClient restClient = null;
            try
            {
                restClient = GetRestClient();
                response = ProcessRequest(restClient, lattitude, longitude, radius);
            }
            catch (RestClientNotFoundException restClientNotFoundException)
            {
                _logger.LogError(restClientNotFoundException.Message);
                response = null;
            }

            if (response == null) throw new InvalidPlaceException($"Place with lattitude {lattitude}" +
                                                                  $" and longitude {longitude} not found");
            return response;
        }

        private HotelList ProcessRequest(RestClient restClient, string lattitude, string longitude, int radius)
        {
            var request = RequestCreator.CreateRequest(restClient, lattitude, longitude, radius);
            var response = restClient.Execute(request);
            var result = JsonConvert.DeserializeObject<HotelList>(response.Content);
            return result;
        }

        private RestClient GetRestClient()
        {
            var client = _hotelService.GetRestClient();
            return (client != null) ? client : throw new RestClientNotFoundException("Could not create rest client");
        }
    }
}
