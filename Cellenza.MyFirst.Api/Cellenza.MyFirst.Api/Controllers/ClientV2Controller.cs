using System.Collections.Generic;
using System.Linq;
using Cellenza.MyFirst.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Cellenza.MyFirst.Api.Controllers
{
    [Route("client")]
    [Route("v2/client")]
    public class ClientV2Controller : Controller
    {
        private readonly ClientDomain clientDomain;

        public ClientV2Controller(ClientDomain clientDomain)
        {
            this.clientDomain = clientDomain;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ClientV2Dto> Get()
        {
            return this.clientDomain.GetAll().Select(ConvertToDto);
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
        public void Post([FromBody]string value)
        {
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
}