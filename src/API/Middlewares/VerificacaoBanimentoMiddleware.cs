using Application.Common.Interfaces;
using System.Security.Claims;

namespace API.Middlewares
{
    public class VerificacaoBanimentoMiddleware
    {
        private readonly RequestDelegate _next;

        public VerificacaoBanimentoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext contexto, IUserPenaltyService servicoPenalidade)
        {
            var identificador = contexto.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                contexto.Connection.RemoteIpAddress?.ToString();

            if (!string.IsNullOrWhiteSpace(identificador))
            {
                var isBanido = await servicoPenalidade.IsBanidoAsync(identificador, contexto.RequestAborted);

                if (isBanido)
                {
                    contexto.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await contexto.Response.WriteAsync("Acesso bloqueado devido a violações recorrentes de limite de taxa.");
                    return; // Retorna imediatamente, barrando a requisição de chegar aos Controllers
                }
            }

            // Se não estiver banido, segue o fluxo normal do pipeline
            await _next(contexto);
        }
    }
}
