using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ML.FichaTecnica
{
    /// <summary>
    /// Middleware para generar Activity Id antes de cada request.
    /// </summary>
    public class MiddlewareHttpModule 
    {
        private readonly RequestDelegate _next;

        public MiddlewareHttpModule(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();

            await this._next.Invoke(context).ConfigureAwait(false);

            // Do some response logic here.
        }
    }
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddlewareHttpModule>();
        }
    }
}
