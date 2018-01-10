using System;
using System.Threading.Tasks;
using Cellenza.MyFirst.Client;

namespace Cellenza.MyFirst.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.WaitAny(ShowClients());

            System.Console.ReadLine();
        }

        public static async Task ShowClients()
        {
            var clientConfig = new ClientConfig();
            clientConfig.BaseUri = "http://localhost:63370/";
            var client = new ClientApi(clientConfig);

            foreach (var clientDto in (await client.GetAll()))
            {
                System.Console.WriteLine($"DisplayName:{clientDto.Name}");
            }
        }
    }
}
