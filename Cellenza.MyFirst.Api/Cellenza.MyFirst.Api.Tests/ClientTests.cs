using System;
using System.Linq;
using System.Security.Claims;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Cellenza.MyFirst.Api.Controllers;
using Cellenza.MyFirst.Domain;
using Cellenza.MyFirst.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace Cellenza.MyFirst.Api.Tests
{
    [TestFixture]
    public class ClientTests
    {
        private MyFirstDbContext myFirstDbContext;
        private ClientV2Controller clientV2Controller;

        [SetUp]
        public void Setup()
        {
            var dbContextObtionsBuilder = new DbContextOptionsBuilder<MyFirstDbContext>();
            dbContextObtionsBuilder.UseInMemoryDatabase("MyFirstDatabase" + Guid.NewGuid());
            myFirstDbContext = new MyFirstDbContext(null);

            var clientDomain = new ClientDomain(myFirstDbContext);
            var userIdentity = Substitute.For<IUserIdentity>();
            this.clientV2Controller = new ClientV2Controller(null, clientDomain, userIdentity);

            var identity = new ClaimsIdentity("Password", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            identity.AddClaim(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                "agaltier",
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                "agaltier",
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(
                OpenIdConnectConstants.Claims.Subject,
                "agaltier",
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(
                "tokenId",
                "1235453432FSD",
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);

            userIdentity.Get(Arg.Any<Controller>()).Returns(identity);
        }

        [Test]
        public void Should_get_all_clients_When_get_api()
        {
            myFirstDbContext.Clients.Add(new Client() { DisplayName = "Aurélien GALTIER", UserLogin = "agaltier" });
            myFirstDbContext.Clients.Add(new Client() { DisplayName = "Toto GALTIER", UserLogin = "agaltier" });
            myFirstDbContext.Clients.Add(new Client() { DisplayName = "Titi GALTIER", UserLogin = "regis" });
            myFirstDbContext.SaveChanges();

            var clients = this.clientV2Controller.Get().ToList();

            Check.That(clients).Not.IsEmpty();
            Check.That(clients.Extracting("DisplayName")).ContainsExactly("Aurélien GALTIER", "Toto GALTIER");
        }

        [Test]
        public void Should_return_client_When_post_to_api()
        {
            var client = this.clientV2Controller.Post("Titi");

            Check.That(client).IsNotNull().And.IsEqualTo(new ClientV2Dto()
            {
                Id = 1,
                DisplayName = "Titi",
            });
        }

        [Test]
        public void Should_save_client_When_post_to_api()
        {
            var client = this.clientV2Controller.Post("Titi");

            Check.That(myFirstDbContext.Clients.ToList().Extracting("DisplayName"))
                .ContainsExactly("Titi");

            Check.That(myFirstDbContext.Clients.ToList().Extracting("UserLogin"))
                .ContainsExactly("agaltier");
        }
    }
}
