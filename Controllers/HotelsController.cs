using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HotelAPI.Cache;
using HotelAPI.Exceptions;
using HotelAPI.HotelAPI.Core.Service;
using HotelAPI.Modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private ILogger<HotelsController> _logger;

        private IDistributedCache _distributedCache;

        private HotelService _hotelService;

        public HotelsController(HotelService hotelService, IDistributedCache distributedCache
                                ,ILogger<HotelsController> logger)
        {
             _logger = logger;
            _distributedCache = distributedCache;
            _hotelService = hotelService;
        }

        private static readonly HttpClient client = new HttpClient();

        [HttpGet("{lattitude}/{longitude}/{radius}")]
        public async Task<ActionResult<List<Hotel>>> Get(string lattitude,string longitude, int radius)
        {

            string dataFromCache = "";
            if (_distributedCache != null)
                dataFromCache = await Redis.GetObjectAsync(_distributedCache, lattitude + "," + longitude + "," + radius);
            
            if (dataFromCache != null)
            {
                var result= JsonConvert.DeserializeObject<List<Hotel>>(dataFromCache);
                return result;
            }
            List<Hotel> response = new List<Hotel>();

            try
            {
                response = GetResponse(lattitude, longitude, radius);
            }
            catch (InvalidPlaceException ipe)
            {
                _logger.LogError(ipe.Message);
            }
            if(_distributedCache != null)
                await Redis.SetObjectAsync(_distributedCache, lattitude + "," + longitude + "," + radius, JsonConvert.SerializeObject(response));
           // dataFromCache = response;
            return response;
        }
        private List<Hotel> GetResponse(string lattitude, string longitude, int radius)
        {
            //string response = "";
            List<Hotel> response = new List<Hotel>();
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

        private List<Hotel> ProcessRequest(RestClient restClient, string lattitude, string longitude, int radius)
        {
            var request = RequestCreator.CreateRequest(restClient, lattitude, longitude, radius);
            var response = restClient.Execute(request);
            var result = JsonConvert.DeserializeObject<List<Hotel>>(response.Content);
            return result;
            //return response.Content;
        }

        private RestClient GetRestClient()
        {
            var client = _hotelService.GetRestClient();
            return (client != null) ? client : throw new RestClientNotFoundException("Could not create rest client");
        }
    }
}
