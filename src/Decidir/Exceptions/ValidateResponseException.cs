using Decidir.Model;

namespace Decidir.Exceptions
{
    public class ValidateResponseException : ResponseException
    {
        public ValidateResponse ValidateResponse { get; private set; }

        public ValidateResponseException(string message) : base(message) { }
        public ValidateResponseException(string message, ErrorResponse errorResponse) : base(message, errorResponse) { }
        public ValidateResponseException(string message, string response) : base(message, response) { }
        public ValidateResponseException(string message, ValidateResponse validateResponse) : base(message)
        {
            ValidateResponse = validateResponse;
        }
    }
}
