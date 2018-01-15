using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cellenza.MyFirst.Client;
using Cellenza.MyFirst.Dto;
using NSubstitute;
using NUnit.Framework;

namespace Cellenza.MyFirst.Console.Tests
{

    [TestFixture]
    public class ClientsLoggerTests
    {
        [Test]
        public async Task Should_display_name_of_clients_When_log_clients()
        {
            var fileLogger = Substitute.For<IFileLogger>();
            var clientApi = Substitute.For<IClientApi>();
            clientApi.GetAll().Returns(new List<ClientV2Dto> { new ClientV2Dto { DisplayName = "MonDisplayName" } });
            var authApi = Substitute.For<IAuthApi>();
            authApi.Connect(Arg.Any<string>(), Arg.Any<string>()).Returns(new Token() {AccessToken = "MonAccessToken"});
            var clientsLogger = new ClientsLogger(fileLogger, authApi, new AuthConfig(), clientApi);

            await clientsLogger.LogClients();

            fileLogger.Received(1).Info("DisplayName:MonDisplayName");
        }
    }
}
