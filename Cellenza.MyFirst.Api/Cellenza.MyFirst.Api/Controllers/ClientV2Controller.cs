using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Cellenza.MyFirst.Api.Infrastructures;
using Cellenza.MyFirst.Domain;
using Cellenza.MyFirst.Dto;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cellenza.MyFirst.Api.Controllers
{
    [Route("client")]
    [Route("v2/client")]
    [Authorize()]
    public class ClientV2Controller : Controller
    {
        private readonly TelemetryClient telemetry;
        private readonly ClientDomain clientDomain;
        private readonly IUserIdentity userIdentity;
        private readonly ICache cache;

        public ClientV2Controller(
            TelemetryClient telemetry,
            ClientDomain clientDomain,
            IUserIdentity userIdentity,
            ICache cache)
        {
            this.telemetry = telemetry;
            this.clientDomain = clientDomain;
            this.userIdentity = userIdentity;
            this.cache = cache;
        }

        // GET api/values
        [HttpGet]
        public ClientV2Dto[] Get(int? index = null, int? take = null, string sort = null)
        {
            if (!index.HasValue)
            {
                index = 0;
            }

            if (index < 0)
            {
                index = 0;
            }

            if (!take.HasValue)
            {
                take = 20;
            }

            if (take > 50)
            {
                take = 50;
            }

            if (string.IsNullOrEmpty(sort))
            {
                sort = "DisplayName";
            }

            var userName = this.userIdentity.Get(this).Name;

            var key = $"{this.HttpContext.Request.Host.Host}/api/v2/client_{userName}_{index}_{take}_{sort}";

            if (this.cache.TryGet<ClientV2Dto[]>(key, out var clientsCache))
            {
                return clientsCache;
            }

            var clients = GetClients(index, take, sort, userName);

            this.cache.Insert(key, clients);

            return clients;

        }

        private ClientV2Dto[] GetClients(int? index, int? take, string sort, string name)
        {
            switch (sort)
            {
                case "DisplayName":
                    return this.clientDomain
                        .GetAllFor(name, index.Value, take.Value, c => c.DisplayName)
                        .Select(ConvertToDto).ToArray();
                case "Id":
                    return this.clientDomain.GetAllFor(name, index.Value, take.Value, c => c.Id)
                        .Select(ConvertToDto).ToArray();
                default:
                    throw new ArgumentException();
            }
        }

        private ClientV2Dto ConvertToDto(Client arg)
        {
            return new ClientV2Dto
            {
                Id = arg.Id,
                DisplayName = arg.DisplayName,
                Url = ""
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public ClientV2Dto Post([FromBody]string displayName)
        {
            var userName = this.userIdentity.Get(this).Name;

            telemetry.TrackEvent("AddClient", new Dictionary<string, string>() { { "name", displayName } });

            return ConvertToDto(this.clientDomain.Save(userName, displayName));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public interface IUserIdentity
    {
        ClaimsIdentity Get(Controller controller);
    }

    public class UserIdentity : IUserIdentity
    {
        public ClaimsIdentity Get(Controller controller)
        {
            return (ClaimsIdentity)controller.User.Identity;
        }
    }
}