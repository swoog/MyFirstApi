using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public Client Save(string userLogin, string displayName)
        {
            var client = new Client();
            client.DisplayName = displayName;
            client.UserLogin = userLogin;
            this.db.Clients.Add(client);
            this.db.SaveChanges();

            return client;
        }

        public IEnumerable<Client> GetAllFor<TKey>(string name, int index, int take, Expression<Func<Client, TKey>> sort)
        {
            return this.db.Clients.Where(c => c.UserLogin == name).OrderBy(sort).Skip(index).Take(take).ToList();
        }
    }
}
