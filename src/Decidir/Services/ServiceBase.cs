using Decidir.Constants;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Decidir.Services
{
    internal abstract class ServiceBase
    {
        protected const string CONTENT_TYPE_APP_JSON = "application/json";
        protected const string METHOD_POST = "POST";
        protected const string METHOD_GET = "GET";
        protected const int STATUS_OK = 200;
        protected const int STATUS_CREATED = 201;
        protected const int STATUS_NOCONTENT = 204;

        protected readonly HttpClient _client;
        protected readonly DecidirSettings _settings;

        public ServiceBase(HttpClient client, DecidirSettings settings)
        {
            _client = client;
            _settings = settings;
        }

        protected bool isErrorResponse(int statusCode)
        {
            if (statusCode == 402)
                return false;
            else
                if (statusCode >= 400 && statusCode < 500)
                return true;
            else
                return false;
        }

        protected void SetPrivateHeaders()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("apikey", _settings.SecretKey);
            _client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        }

        protected StringContent CreateContent(object model = null, JsonSerializerSettings settings = null) =>
            new StringContent(
                JsonConvert.SerializeObject(model, Formatting.None, settings ?? new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }),
                Encoding.GetEncoding("iso-8859-1"),
                "application/json");
    }
}
