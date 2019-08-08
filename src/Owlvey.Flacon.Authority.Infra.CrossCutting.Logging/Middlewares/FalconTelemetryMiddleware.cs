using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Owlvey.Flacon.Authority.Infra.CrossCutting.Logging.Middlewares
{
    public sealed class FalconTelemetryHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerOptions _options;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;
        readonly IHostingEnvironment _hostingEnvironment;
        public FalconTelemetryHandlerMiddleware(
            IOptions<ExceptionHandlerOptions> options,
            RequestDelegate next,
            DiagnosticSource diagSource,
            IHostingEnvironment hostingEnvironment)
        {
            _options = options.Value;
            _next = next;
            if (_options.ExceptionHandler == null)
            {
                _options.ExceptionHandler = _next;
            }
            _clearCacheHeadersDelegate = ClearCacheHeaders;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //Log.Error($"There was an unexpected error: {ex.ToString()}");
                //Log.CloseAndFlush();

                PathString originalPath = context.Request.Path;
                if (_options.ExceptionHandlingPath.HasValue)
                {
                    context.Request.Path = _options.ExceptionHandlingPath;
                }

                context.Response.Clear();
                var exceptionHandlerFeature = new ExceptionHandlerFeature()
                {
                    Error = ex,
                    Path = originalPath.Value,
                };

                context.Features.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);
                context.Features.Set<IExceptionHandlerPathFeature>(exceptionHandlerFeature);
                context.Response.StatusCode = 500;
                context.Response.OnStarting(_clearCacheHeadersDelegate, context.Response);

                await _options.ExceptionHandler(context);

                return;
            }
        }

        private Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }
    }
}
