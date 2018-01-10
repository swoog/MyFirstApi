using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cellenza.MyFirst.Dto;
using Pattern.Api;

namespace Cellenza.MyFirst.Client
{
    public class ClientConfig
    {
        public string BaseUri { get; set; }
    }

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

    public class ClientApi : MyFirstBaseApi, IClientApi
    {
        public ClientApi(ClientConfig config) : base(config)
        {
        }

        public Task<List<ClientDto>> GetAll()
        {
            return this.GetAsync<List<ClientDto>>("/v1/client/");
        }
    }
}
