using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cellenza.MyFirst.Domain;
using Cellenza.MyFirst.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cellenza.MyFirst.Api.Controllers
{
    [Route("v1/client")]
    //[Authorize("client.read")]
    public class ClientController : Controller
    {
        private readonly ClientDomain clientDomain;

        public ClientController(ClientDomain clientDomain)
        {
            this.clientDomain = clientDomain;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ClientDto> Get()
        {
            return this.clientDomain.GetAll().Select(ConvertToDto);
        }

        private ClientDto ConvertToDto(Client arg)
        {
            return new ClientDto
            {
                Id = arg.Id,
                Name = arg.DisplayName,
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
        //[Authorize("client.write")]
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
