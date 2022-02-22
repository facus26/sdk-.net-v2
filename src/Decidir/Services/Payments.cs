using Decidir.Constants;
using Decidir.Exceptions;
using Decidir.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Decidir.Services
{
    internal class Payments : ServiceBase
    {
        public Payments(HttpClient client, DecidirSettings settings) : base(client, settings) { }

        public async Task<PaymentResponse> ExecutePayment(OfflinePayment payment) =>
            await DoPayment(payment.copyOffline());

        public async Task<PaymentResponse> ExecutePayment(Payment payment) =>
            await DoPayment(payment.copy());

        public async Task<CapturePaymentResponse> CapturePayment(long paymentId, double amount)
        {
            SetPrivateHeaders();
            var res = await _client.PutAsync($"api/v2/payments/{paymentId}", CreateContent(new { amount = Convert.ToInt32(amount * 100).ToString() }));

            if (res.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<CapturePaymentResponse>(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ResponseException($"{res.StatusCode} - {content}");
        }

        public async Task<GetAllPaymentsResponse> GetAllPayments(long? offset = null, long? pageSize = null, string siteOperationId = null, string merchantId = null)
        {
            var queryString = GetAllPaymentsQuery(offset, pageSize, siteOperationId, merchantId);

            var path = string.IsNullOrEmpty(queryString)
                ? "api/v2/payments"
                : $"api/v2/payments?{queryString}";

            SetPrivateHeaders();
            var res = await _client.GetAsync(path);

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ResponseException($"{res.StatusCode} - {content}", content);
        }

        public async Task<PaymentResponse> GetPaymentInfo(long paymentId)
        {
            SetPrivateHeaders();
            var res = await _client.GetAsync($"api/v2/payments/{paymentId}?expand=card_data");

            if (res.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<PaymentResponseExtend>(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ResponseException($"{res.StatusCode} - {content}", content);
        }

        public async Task<RefundResponse> Refund(long paymentId)
        {
            SetPrivateHeaders();
            var res = await _client.PostAsync($"api/v2/payments/{paymentId}/refunds", CreateContent());

            if (res.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<RefundResponse>(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ResponseException($"{res.StatusCode} - {content}", content);
        }

        public async Task<DeleteRefundResponse> DeleteRefund(long paymentId, long refundId)
        {
            SetPrivateHeaders();
            var res = await _client.DeleteAsync($"api/v2/payments/{paymentId}/refunds/{refundId}");

            if (res.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<DeleteRefundResponse>(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ResponseException($"{res.StatusCode} - {content}", content);
        }

        public async Task<RefundResponse> PartialRefund(long paymentId, double amount)
        {
            SetPrivateHeaders();
            var res = await _client.PostAsync($"api/v2/payments/{paymentId}/refunds", CreateContent(new { amount = Convert.ToInt64(amount * 100) }));

            if (res.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<RefundResponse>(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ResponseException($"{res.StatusCode} - {content}", content);
        }

        public async Task<DeleteRefundResponse> DeletePartialRefund(long paymentId, long refundId) =>
            await DeleteRefund(paymentId, refundId);

        protected async Task<PaymentResponse> DoPayment(Payment payment)
        {
            payment.ConvertDecidirAmounts();

            SetPrivateHeaders();
            var res = await _client.PostAsync("api/v2/payments", CreateContent(payment));

            if (res.IsSuccessStatusCode)
            {
                var response = JsonConvert.DeserializeObject<PaymentResponse>(await res.Content.ReadAsStringAsync());
                response.statusCode = (int)res.StatusCode;

                return response;
            }

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new PaymentResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new PaymentResponseException($"{res.StatusCode} - {content}", content);
        }

        private string GetAllPaymentsQuery(long? offset, long? pageSize, string siteOperationId, string merchantId)
        {
            StringBuilder result = new StringBuilder();

            if (offset != null)
                result.Append(string.Format("{0}={1}", "offset", offset));

            if (pageSize != null)
                result.Append(string.Format("{0}={1}", "pageSize", pageSize));

            if (!string.IsNullOrEmpty(siteOperationId))
                result.Append(string.Format("{0}={1}", "siteOperationId", siteOperationId));

            if (!string.IsNullOrEmpty(merchantId))
                result.Append(string.Format("{0}={1}", "merchantId", merchantId));

            return result.ToString();
        }
    }
}
