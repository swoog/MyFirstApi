using System;
using System.Threading.Tasks;
using Cellenza.MyFirst.Client;

namespace Cellenza.MyFirst.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.WaitAll(ShowClients());

            System.Console.ReadLine();
        }

        public static async Task ShowClients()
        {
            var clientConfig = new ClientConfig
            {
                BaseUri = "https://localhost:44392/"
            };

            System.Console.WriteLine($"Authenticate...");
            var auth = new AuthApi(clientConfig);

            var token = await auth.Connect("aurelien", "12345");

            var authConfig = new AuthConfig {AccessToken = token.AccessToken};

            System.Console.WriteLine($"Authenticate Done");
            var client = new ClientApi(clientConfig, authConfig);

            foreach (var clientDto in (await client.GetAll()))
            {
                System.Console.WriteLine($"DisplayName:{clientDto.DisplayName}");
            }
        }
    }
}
