using Decidir.Constants;
using Decidir.Exceptions;
using Decidir.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Decidir.Services
{
    internal class CardTokens : ServiceBase
    {
        public CardTokens(HttpClient client, DecidirSettings settings) : base(client, settings) { }

        public async Task<bool> DeleteCardToken(string tokenizedCard)
        {
            SetPrivateHeaders();
            var res = await _client.DeleteAsync($"api/v2/cardtokens/{tokenizedCard}");

            if (res.IsSuccessStatusCode)
                return true;

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ResponseException($"{res.StatusCode} - {content}", content);
        }
    }
}
