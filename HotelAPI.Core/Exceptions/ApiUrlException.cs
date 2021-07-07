using System;
using System.Runtime.Serialization;

namespace HotelAPI.HotelAPI.Core.Exceptions
{
    [Serializable]
    public class ApiUrlException : Exception
    {
        public ApiUrlException()
        {
        }

        public ApiUrlException(string message) : base(message)
        {
        }

        public ApiUrlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}