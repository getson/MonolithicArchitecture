﻿using BinaryOrigin.SeedWork.Core.Exceptions;
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            switch (exception)
            {
                case EntityDoesNotExistException _:
                    code = HttpStatusCode.NotFound;
                    break;

                case UnAuthorizedException _:
                    code = HttpStatusCode.Unauthorized;
                    break;

                case ValidationException _:
                    code = HttpStatusCode.BadRequest;
                    break;

                case GeneralConflictException _:
                    code = HttpStatusCode.Conflict;
                    break;

                case GeneralException _:
                    code = HttpStatusCode.BadRequest;
                    break;

                case ArgumentException _:
                    code = HttpStatusCode.BadRequest;
                    break;
            }

            var result = JsonConvert.SerializeObject(new
            {
                error = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}