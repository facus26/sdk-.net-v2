using Decidir.Constants;
using Decidir.Exceptions;
using Decidir.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Decidir.Services
{
    internal class HealthCheck : ServiceBase
    {
        public HealthCheck(HttpClient client, DecidirSettings settings) : base(client, settings) { }

        public async Task<HealthCheckResponse> Execute()
        {
            var res = await _client.GetAsync("api/v2/healthcheck");

            if (res.IsSuccessStatusCode)
                return HealthCheckResponse.toHealthCheckResponse(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (isErrorResponse((int)res.StatusCode))
                throw new ResponseException(res.StatusCode.ToString(), JsonConvert.DeserializeObject<ErrorResponse>(content));
            else
                throw new ResponseException($"{res.StatusCode} - {content}", content);
        }
    }
}
