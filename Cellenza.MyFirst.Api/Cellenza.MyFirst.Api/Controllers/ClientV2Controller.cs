using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
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

        public ClientV2Controller(TelemetryClient telemetry, ClientDomain clientDomain, IUserIdentity userIdentity)
        {
            this.telemetry = telemetry;
            this.clientDomain = clientDomain;
            this.userIdentity = userIdentity;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ClientV2Dto> Get(int? index = null, int? take = null, string sort = null)
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

            switch (sort)
            {
                case "DisplayName":
                    return this.clientDomain.GetAllFor(this.userIdentity.Get(this).Name, index.Value, take.Value, c => c.DisplayName)
                        .Select(ConvertToDto);
                case "Id":
                    return this.clientDomain.GetAllFor(this.userIdentity.Get(this).Name, index.Value, take.Value, c=>c.Id)
                        .Select(ConvertToDto);
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
            telemetry.TrackEvent("AddClient", new Dictionary<string, string>() { { "name", displayName } });
            return ConvertToDto(this.clientDomain.Save(this.userIdentity.Get(this).Name, displayName));
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