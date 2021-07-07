using System;
using System.Runtime.Serialization;

namespace HotelAPI.HotelAPI.Core.Exceptions
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