using System;
using System.Collections.Generic;
using System.Linq;

namespace Cellenza.MyFirst.Domain
{
    public class ClientDomain
    {
        private readonly MyFirstDbContext db;

        public ClientDomain(MyFirstDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Client> GetAll()
        {
            return this.db.Clients.ToList();
        }

    }
}
