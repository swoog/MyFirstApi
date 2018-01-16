using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cellenza.MyFirst.Api
{
    public static class BlobStorageMiddleWare
    {
        public static IApplicationBuilder UseBlobStorage(this IApplicationBuilder app)
        {
            app.Use(HostBlobStorage);
            return app;
        }

        private static async Task HostBlobStorage(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path.StartsWithSegments("/storage/swagger"))
            {
                // Send help HTML file
            }
            else if (context.Request.Path.StartsWithSegments("/storage"))
            {
                if (context.Request.Method == "POST")
                {
                    // Write file
                }
                else
                {
                    // Read file
                }
            }
            else
            {
                await next.Invoke();
            }
        }
    }
}