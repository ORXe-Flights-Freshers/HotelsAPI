using System;
using System.Runtime.Serialization;

namespace HotelAPI.HotelAPI.Core.Exceptions
{
    [Serializable]
    public class RestClientNotFoundException : Exception
    {
        public RestClientNotFoundException()
        {
        }

        public RestClientNotFoundException(string message) : base(message)
        {
        }

        public RestClientNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RestClientNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}