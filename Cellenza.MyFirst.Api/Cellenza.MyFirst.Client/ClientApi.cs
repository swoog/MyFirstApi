using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cellenza.MyFirst.Dto;

namespace Cellenza.MyFirst.Client
{
    public class ClientApi : AuthMyFirstBaseApi, IClientApi
    {
        public ClientApi(ClientConfig config, AuthConfig authConfig) : base(config, authConfig)
        {
        }

        public Task<List<ClientV2Dto>> GetAll()
        {
            return this.GetAsync<List<ClientV2Dto>>("/v2/client/");
        }
    }
}
