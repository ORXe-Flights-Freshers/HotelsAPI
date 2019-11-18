using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI
{
    public static class RequestCreator
    {
        public static RestRequest CreateRequest(RestClient restClient, string lattitude, string longitude, int radius)
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
            request.AddParameter("undefined", "{  \n    \"georegion\": {\n    \"circle\": {\n      \"center\": {\n        \"lat\":  " + lattitude + ",\n        \"long\":  " + longitude + "\n      },\n      \"radiusKm\": " + radius + "\n    }\n  },\n  \"supplierFamilies\": [\n    \"ean\"\n  ],\n  \"contentPrefs\": [\n    \"Basic\"\n  ]\n}", ParameterType.RequestBody);
            return request;

        }
    }
}
