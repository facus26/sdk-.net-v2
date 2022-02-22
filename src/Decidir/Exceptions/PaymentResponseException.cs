using Decidir.Model;

namespace Decidir.Exceptions
{
    public class PaymentResponseException : ResponseException
    {
        public PaymentResponse PaymentResponse { get; private set; }

        public PaymentResponseException(string message) : base(message) { }
        public PaymentResponseException(string message, string response) : base(message, response) { }
        public PaymentResponseException(string message, ErrorResponse errorResponse) : base(message, errorResponse) { }
        public PaymentResponseException(string message, PaymentResponse paymentResponse) : base(message)
        {
            PaymentResponse = paymentResponse;
        }
    }
}
