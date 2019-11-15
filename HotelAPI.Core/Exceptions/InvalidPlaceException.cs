using System;
using System.Runtime.Serialization;

namespace HotelAPI.Controllers
{
    [Serializable]
    internal class InvalidPlaceException : Exception
    {
        public InvalidPlaceException()
        {
        }

        public InvalidPlaceException(string message) : base(message)
        {
        }

        public InvalidPlaceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPlaceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}