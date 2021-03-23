using Anazon.Application.Helpers;
using Anazon.Domain.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Anazon.Presentation.Api
{
    public static class Middlewares
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature is not null)
                    {

                        var result = (ResultHelper.Error(exceptionHandlerFeature.Error) as ObjectResult).Value as Result;
                        var content = JsonSerializer.Serialize(result);

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = result.StatusCode;

                        await context.Response.WriteAsync(content);
                    }
                });
            });
        }
    }
}
