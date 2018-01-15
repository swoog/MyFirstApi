using System.Net.Http;
using Pattern.Api;

namespace Cellenza.MyFirst.Client
{
    public class MyFirstBaseApi : BaseApi
    {
        private readonly ClientConfig config;

        public MyFirstBaseApi(ClientConfig config)
        {
            this.config = config;
        }

        protected override string GetBaseUrl()
        {
            return this.config.BaseUri;
        }
    }

    public class AuthMyFirstBaseApi : MyFirstBaseApi
    {
        private readonly AuthConfig authConfig;

        public AuthMyFirstBaseApi(ClientConfig config, AuthConfig authConfig) : base(config)
        {
            this.authConfig = authConfig;
        }

        protected override HttpClient CreateClient()
        {
            var httpClient = base.CreateClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authConfig.AccessToken);

            return httpClient;
        }
    }
}