using Decidir.Constants;
using Decidir.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Decidir.Services
{
    internal class UserSite : ServiceBase
    {
        public UserSite(HttpClient client, DecidirSettings settings) : base(client, settings) { }

        public async Task<GetAllCardTokensResponse> GetAllTokens(string userId)
        {
            var res = await _client.GetAsync($"api/v2/usersite/{userId}/cardtokens");

            if (res.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<GetAllCardTokensResponse>(await res.Content.ReadAsStringAsync());

            return null;
        }
    }
}
