using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Flacon.Authority.Infra.CrossCutting.Logging.Middlewares
{
    public static class FalconTelemetryMiddlewareExtensions
    {
        public static IApplicationBuilder UseFalconLogging(this IApplicationBuilder builder, string errorHandlingPath)
        {
            return builder.UseMiddleware<FalconTelemetryHandlerMiddleware>
                    (Options.Create(new ExceptionHandlerOptions
                    {
                        ExceptionHandlingPath = new PathString(errorHandlingPath)
                    }));
        }
    }
}
