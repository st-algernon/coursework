using System.Net;
using Coursework_server.Extensions.Middleware.Base;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Coursework_server.Extensions.Middleware
{
    public class ExceptionSerializerMiddleware : BaseMiddleware
    {
        public ExceptionSerializerMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override async Task Invoke(HttpContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case UnauthorizedAccessException _:
                        await SerializeException(context, HttpStatusCode.Forbidden, e);
                        break;
                    case DbUpdateConcurrencyException updateConcurrencyException:
                        await SerializeException(context, HttpStatusCode.Conflict, updateConcurrencyException);
                        break;
                    case DbUpdateException dbUpdateException:
                        await SerializeException(context, HttpStatusCode.Conflict, dbUpdateException, dbUpdateException.InnerException?.Message);
                        break;
                    case InvalidOperationException invalidOperationException:
                        await SerializeException(context, HttpStatusCode.NotFound, invalidOperationException);
                        break;
                    default:
                        await SerializeException(context, HttpStatusCode.InternalServerError, e, "An error occurred while processing your request");
                        break;
                }
            }
        }

        private static Task SerializeException(HttpContext context, HttpStatusCode statusCode, Exception exception, object? data = null, string? message = null)
        {
            var response = context.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";

            var error = new ErrorDescription
            {
                Message = message ?? exception.Message,
                Data = data ?? exception.StackTrace
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return context.Response.WriteAsync(JsonConvert.SerializeObject(error, settings));
        }
    }

    internal class ErrorDescription
    {
        public object? Data { get; set; }
        public string? Message { get; set; }
    }

    public static class ExceptionSerializerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionSerializerMiddleware(this IApplicationBuilder source)
        {
            source.UseMiddleware<ExceptionSerializerMiddleware>();
            return source;
        }
    }   
}