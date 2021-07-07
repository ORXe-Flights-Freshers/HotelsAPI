using System.Net.Http;
using System.Threading.Tasks;
using HotelAPI.HotelAPI.Core.Service;
using HotelAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private HotelResponse _hotelResponse;
        public HotelsController(HotelResponse hotelResponse)
        {
            _hotelResponse = hotelResponse;
        }

        private static readonly HttpClient client = new HttpClient();

        [HttpGet("{lattitude}/{longitude}/{radius}")]
        public async Task<ActionResult<HotelList>> Get(string lattitude,string longitude, int radius)
        {
            return await _hotelResponse.Get(lattitude, longitude, radius);
        }
        
    }
}
