using System;
using System.Runtime.Serialization;

namespace AjandekListaApp
{
    [Serializable]
    internal class sqlNullValueException : Exception
    {
        public sqlNullValueException()
        {
        }

        public sqlNullValueException(string message) : base(message)
        {
        }

        public sqlNullValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected sqlNullValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}