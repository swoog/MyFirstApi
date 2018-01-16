using System;
using System.Threading.Tasks;
using Cellenza.MyFirst.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cellenza.MyFirst.Api
{
    public static class HostnameMiddleWare
    {
        public static IApplicationBuilder UseHostname(this IApplicationBuilder app)
        {
            app.Use(HostNameMiddle);
            return app;
        }

        private static async Task HostNameMiddle(HttpContext context, Func<Task> next)
        {
            var databaseConfig = context.RequestServices.GetService<DatabaseConfig>();
            var hostname = context.Request.Host.Host;

            if (hostname != "localhost")
            {
                databaseConfig.DataBaseName = hostname;
            }
            else
            {
                databaseConfig.DataBaseName = "MyFirstDatabase";
            }

            await next.Invoke();
        }
    }
}