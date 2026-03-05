using Application.Common.Exceptions;
using Application.Common.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ocorreu uma exceção: {Message}", exception.Message);

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errorResponse = Erro.Default;

            if (exception is DomainException domainEx)
            {
                statusCode = domainEx.Error.Codigo;
                errorResponse = domainEx.Error;
            }
            else if (exception is ValidationException validationEx)
            {
                statusCode = validationEx.Error.Codigo;
                errorResponse = validationEx.Error;
            }

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(Resposta.Failure(errorResponse), jsonOptions);

            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true;
        }
    }
}