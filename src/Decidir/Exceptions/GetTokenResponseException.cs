using Decidir.Model;

namespace Decidir.Exceptions
{
    public class GetTokenResponseException : ResponseException
    {
        public GetTokenResponse TokenResponse { get; private set; }

        public GetTokenResponseException(string message) : base(message) { }
        public GetTokenResponseException(string message, string response) : base(message, response) { }
        public GetTokenResponseException(string message, ErrorResponse errorResponse) : base(message, errorResponse) { }
        public GetTokenResponseException(string message, GetTokenResponse getTokenResponse) : base(message)
        {
            TokenResponse = getTokenResponse;
        }
    }
}