using System;
using System.Collections.Generic;
using System.Text;

namespace VtConnect.Exceptions
{
    public class VtConnectException : Exception
    {
        public VtConnectException(string message) :
            base(message)
        {
        }

        public VtConnectException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
