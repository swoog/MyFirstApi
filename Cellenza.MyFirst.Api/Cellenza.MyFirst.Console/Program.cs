using System.Threading.Tasks;
using Cellenza.MyFirst.Client;
using Ninject;

namespace Cellenza.MyFirst.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            kernel.Bind<ClientConfig>().ToMethod(c => new ClientConfig()
            {
                BaseUri = "https://localhost:44392/"
            }).InSingletonScope();

            kernel.Bind<IAuthApi>().To<AuthApi>();
            kernel.Bind<IFileLogger>().To<FileLogger>();
            kernel.Bind<AuthConfig>().To<AuthConfig>()
                .InSingletonScope();
            kernel.Bind<IClientApi>().To<ClientApi>();
            kernel.Bind<ClientsLogger>().To<ClientsLogger>();

            var clientsLogger = kernel.Get<ClientsLogger>();

            Task.WaitAll(clientsLogger.LogClients());

            System.Console.ReadLine();
        }
    }
}
