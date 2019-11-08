using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI.Cache
{
    public class Redis
    {
        public static async Task SetObjectAsync(IDistributedCache cache, string key, string value)
        {
            await cache.SetStringAsync(key, value);
        }

        public static async Task<string> GetObjectAsync(IDistributedCache cache, string key)
        {
            string value = await cache.GetStringAsync(key);
            return value;
        }
    }
}
