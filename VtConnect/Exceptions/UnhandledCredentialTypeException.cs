using System;
using System.Collections.Generic;
using System.Text;

namespace VtConnect.Exceptions
{
    public class UnhandledCredentialTypeException : VtConnectException
    {
        public UnhandledCredentialTypeException(string message) :
            base(message)
        {
        }

        public UnhandledCredentialTypeException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
