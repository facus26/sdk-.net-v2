using Decidir.Constants;
using Decidir.Model;
using Decidir.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace Decidir
{
    public class DecidirConnector
    {
        private readonly HealthCheck _healthCheckService;
        private readonly Payments _paymentService;
        private readonly UserSite _userSiteService;
        private readonly CardTokens _cardTokensService;
        private readonly PaymentValidate _paymentValidateService;

        public DecidirConnector(HttpClient client, DecidirSettings settings)
        {
            _healthCheckService = new HealthCheck(client, settings);
            _paymentService = new Payments(client, settings);
            _userSiteService = new UserSite(client, settings);
            _cardTokensService = new CardTokens(client, settings);
            _paymentValidateService = new PaymentValidate(client, settings);
        }

        public async Task<HealthCheckResponse> HealthCheck() =>
            await _healthCheckService.Execute();

        public async Task<PaymentResponse> Payment(Payment payment) =>
            await _paymentService.ExecutePayment(payment);

        public async Task<PaymentResponse> Payment(OfflinePayment payment) =>
            await _paymentService.ExecutePayment(payment);

        public async Task<CapturePaymentResponse> CapturePayment(long paymentId, double amount) =>
            await _paymentService.CapturePayment(paymentId, amount);

        public async Task<GetAllPaymentsResponse> GetAllPayments(long? offset = null, long? pageSize = null, string siteOperationId = null, string merchantId = null) =>
            await _paymentService.GetAllPayments(offset, pageSize, siteOperationId, merchantId);

        public async Task<PaymentResponse> GetPaymentInfo(long paymentId) =>
            await _paymentService.GetPaymentInfo(paymentId);

        public async Task<RefundResponse> Refund(long paymentId) =>
            await _paymentService.Refund(paymentId);

        public async Task<DeleteRefundResponse> DeleteRefund(long paymentId, long refundId) =>
            await _paymentService.DeleteRefund(paymentId, refundId);

        public async Task<RefundResponse> PartialRefund(long paymentId, double amount) =>
            await _paymentService.PartialRefund(paymentId, amount);

        public async Task<DeleteRefundResponse> DeletePartialRefund(long paymentId, long refundId) =>
            await _paymentService.DeletePartialRefund(paymentId, refundId);

        public async Task<GetAllCardTokensResponse> GetAllCardTokens(string userId) =>
            await _userSiteService.GetAllTokens(userId);

        public async Task<bool> DeleteCardToken(string token) =>
            await _cardTokensService.DeleteCardToken(token);

        public async Task<ValidateResponse> Validate(ValidateData validateData) =>
            await _paymentValidateService.ValidatePayment(validateData);

        public async Task<GetTokenResponse> GetToken(CardTokenBsa card_token_bsa) =>
            await _paymentValidateService.GetToken(card_token_bsa);
    }
}
