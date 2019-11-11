using System;
using System.Runtime.Serialization;

namespace HotelAPI.Core.Exceptions
{
    [Serializable]
    public class InvalidHostException : Exception
    {
        public InvalidHostException()
        {
        }

        public InvalidHostException(string message) : base(message)
        {
        }

        public InvalidHostException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidHostException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}