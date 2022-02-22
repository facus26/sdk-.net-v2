using Decidir.Model;
using System;

namespace Decidir.Exceptions
{
    public class ResponseException : Exception
    {
        public ErrorResponse ErrorResponse { get; private set; }

        public ResponseException(string message) : base(message) { }

        public ResponseException(string message, string response) : base(message)
        {
            Data.Add("Response", response);
        }

        public ResponseException(string message, ErrorResponse errorResponse) : base(message)
        {
            ErrorResponse = errorResponse;
            Data.Add("Response", errorResponse);
        }
    }
}
