using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI.Modal
{
    public class Hotel
    {
        [JsonProperty(PropertyName = "id")]
        public string  HotelId { get; set; }


        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }


        [JsonProperty(PropertyName = "rating")]
        public double rating { get; set; }

        [JsonProperty(PropertyName = "geoCode")]
        public geoCode geocode { get; set; }

        [JsonProperty(PropertyName = "contact")]
        public contact contact { get; set; }

       
    }

    public class geoCode
    {
        public double lat { get; set; }

        [JsonProperty(PropertyName = "long")]
        public double lon { get; set; }

    }
    public class contact
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
    }

}
