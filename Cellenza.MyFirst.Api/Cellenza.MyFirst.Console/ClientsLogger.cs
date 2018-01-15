using System.Threading.Tasks;
using Cellenza.MyFirst.Client;

namespace Cellenza.MyFirst.Console
{
    public class ClientsLogger
    {
        private readonly IFileLogger fileLogger;
        private readonly IAuthApi authApi;
        private readonly AuthConfig authConfig;
        private readonly IClientApi clientApi;

        public ClientsLogger(IFileLogger fileLogger, IAuthApi authApi, AuthConfig authConfig, IClientApi clientApi)
        {
            this.fileLogger = fileLogger;
            this.authApi = authApi;
            this.authConfig = authConfig;
            this.clientApi = clientApi;
        }

        public async Task LogClients()
        {
            fileLogger.Info($"Authenticate...");

            var token = await authApi.Connect("toto", "12345");

            this.authConfig.AccessToken = token.AccessToken;

            fileLogger.Info($"Authenticate Done");

            foreach (var clientDto in (await clientApi.GetAll()))
            {
                fileLogger.Info($"DisplayName:{clientDto.DisplayName}");
            }
        }
    }
}