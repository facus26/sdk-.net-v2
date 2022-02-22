using Decidir.Constants;
using Decidir.Exceptions;
using Decidir.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Decidir.Services
{
    internal class PaymentValidate : ServiceBase
    {
        public PaymentValidate(HttpClient client, DecidirSettings settings) : base(client, settings) { }

        public async Task<ValidateResponse> ValidatePayment(ValidateData validatePayment)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("apikey", _settings.ValidateKey);
            _client.DefaultRequestHeaders.Add("X-Consumer-Username", _settings.Merchant);
            _client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

            var res = await _client.PostAsync("/web/validate", CreateContent(validatePayment));

            if (res.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ValidateResponse>(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ValidateResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ValidateResponseException($"{res.StatusCode} - {content}", JsonConvert.DeserializeObject<ValidateResponse>(content));
        }

        public async Task<GetTokenResponse> GetToken(CardTokenBsa card_token)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("apikey", _settings.PublicKey);

            var res = await _client.PostAsync("api/v2/tokens", CreateContent(card_token));

            if (res.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<GetTokenResponse>(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new GetTokenResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new GetTokenResponseException($"{res.StatusCode} - {content}", JsonConvert.DeserializeObject<GetTokenResponse>(content));
        }
    }
}
