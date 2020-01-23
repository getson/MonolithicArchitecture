using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.WebApi
{
    /// <summary>
    /// Middleware for error handling of api exceptions
    /// For more info https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            object errorMessage = null;

            switch (exception)
            {
                case GeneralException _:
                    code = HttpStatusCode.BadRequest;
                    break;
                case CommandValidationException _:
                    code = HttpStatusCode.BadRequest;
                    errorMessage = exception.Data;
                    break;
                case ArgumentException _:
                    code = HttpStatusCode.BadRequest;
                    break;
            }

            var result = JsonConvert.SerializeObject(new
            {
                errors = errorMessage ?? exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}