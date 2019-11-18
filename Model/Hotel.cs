using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI.Model
{
    public class Hotel
    {
        [JsonProperty(PropertyName = "id")]
        public string HotelId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double Rating { get; set; }

        [JsonProperty(PropertyName = "geoCode")]
        public GeoCode Geocode { get; set; }

        [JsonProperty(PropertyName = "contact")]
        public Contact Contact { get; set; }
    }

    public class GeoCode
    {
        [JsonProperty(PropertyName = "lat")]
        public double Lattitude { get; set; }

        [JsonProperty(PropertyName = "long")]
        public double Longitude { get; set; }

    }
    public class Contact
    {
        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }
    }
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
    }
    public class HotelList
    {
        public List<Hotel> Hotels { get; set; }
    }
}
