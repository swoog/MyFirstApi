using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Cellenza.MyFirst.Api.Controllers;
using Cellenza.MyFirst.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Cellenza.MyFirst.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DatabaseConfig>();

            services.AddDbContext<MyFirstDbContext>();

            var x509Certificate2 = new X509Certificate2(
                $@"{hostingEnvironment.ContentRootPath}\Agile.pfx",
                null as string,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            services.AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // Autnentification Client
                .AddJwtBearer(o =>
                {
                    o.Configuration = new OpenIdConnectConfiguration();
                    o.TokenValidationParameters.ValidateAudience = false;
                    o.TokenValidationParameters.ValidIssuer = Configuration["Settings:Url"];
                    o.Configuration.SigningKeys.Add(new X509SecurityKey(x509Certificate2));
                })
                // Authentification Server
                .AddOpenIdConnectServer(o =>
                {
                    o.AccessTokenHandler = new JwtSecurityTokenHandler
                    {
                        InboundClaimTypeMap = new Dictionary<string, string>(),
                        OutboundClaimTypeMap = new Dictionary<string, string>()
                    };
                    o.TokenEndpointPath = "/connect/token";
                    o.Provider.OnValidateTokenRequest = OnValidateTokenRequest;
                    o.Provider.OnHandleTokenRequest = OnHandleTokenRequest;
                    o.Provider.OnHandleAuthorizationRequest = OnHandleAuthorizationRequest;
                    o.AccessTokenLifetime = TimeSpan.FromDays(360);
                    o.SigningCredentials.AddCertificate(x509Certificate2);
                });

            services.AddScoped<ClientDomain>();
            services.AddScoped<IUserIdentity, UserIdentity>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.SwaggerDoc("v2", new Info { Title = "My API", Version = "v2" });
            });

            services.AddMvc();
        }

        private Task OnHandleAuthorizationRequest(HandleAuthorizationRequestContext context)
        {
            return Task.CompletedTask;
        }

        private async Task OnHandleTokenRequest(HandleTokenRequestContext context)
        {
            // Only handle grant_type=password token requests and let
            // the OpenID Connect server handle the other grant types.
            if (context.Request.GrantType == "Password")
            {
                if (context.Request.Password == "12345")
                {
                    // your choice of claims...
                    ReturnIdentity(context, context.Request.Username);
                }
            }
        }

        private static void ReturnIdentity(HandleTokenRequestContext context, string user)
        {
            var identity = new ClaimsIdentity(context.Scheme.Name, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            identity.AddClaim(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                string.IsNullOrEmpty(user) ? "Anonyme" : user,
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                user,
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(
                OpenIdConnectConstants.Claims.Subject,
                user,
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(
                "tokenId",
                "1235453432FSD",
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);


            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Scheme.Name);

            // Call SetScopes with the list of scopes you want to grant
            // (specify offline_access to issue a refresh token).
            ticket.SetScopes(
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess);

            context.Validate(ticket);
        }

        private Task OnValidateTokenRequest(ValidateTokenRequestContext context)
        {
            if (context.Request.GrantType == "Password")
            {
                context.Skip();

                return Task.CompletedTask;
            }

            context.Reject(
                error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                description: "Only grant_type=password" +
                             "requests are accepted by this server.");

            return Task.CompletedTask;
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHostname();

            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseBlobStorage();

            app.UseMvc();
        }
    }
}
