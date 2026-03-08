using Application.Common.Interfaces;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace API.Extensions
{
    public static class LimiteRequisicoesExtensions
    {
        public static IServiceCollection AddConfiguracaoRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRateLimiter(opcoes =>
            {
                // Limite geral de concorrência (protege o servidor)
                opcoes.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(contexto =>
                    RateLimitPartition.GetConcurrencyLimiter("Global",
                        _ => new ConcurrencyLimiterOptions
                        {
                            PermitLimit = configuration.GetValue<int>("LimiteRequisicoes:ConcorrenciaGlobal"),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = configuration.GetValue<int>("LimiteRequisicoes:FilaConcorrenciaGlobal")
                        }));

                // Limite padrão (particionado por usuário ou IP)
                opcoes.AddPolicy("PoliticaPadrao", contexto =>
                {
                    // Se estiver autenticado, usa o ID do usuário
                    var idUsuario = contexto.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (!string.IsNullOrWhiteSpace(idUsuario))
                    {
                        return RateLimitPartition.GetFixedWindowLimiter(idUsuario, _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = configuration.GetValue<int>("LimiteRequisicoes:UsuarioPorMinuto"),
                            Window = TimeSpan.FromMinutes(1)
                        });
                    }

                    // Se for anônimo, usa o IP
                    var ip = contexto.Connection.RemoteIpAddress?.ToString() ?? "ip_desconhecido";

                    return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = configuration.GetValue<int>("LimiteRequisicoes:IpPorMinuto"),
                        Window = TimeSpan.FromMinutes(1)
                    });
                });

                // Limite específico para endpoints pesados 
                opcoes.AddPolicy("PolicitaEndpoint", contexto =>
                {
                    var identificador =
                        contexto.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                        contexto.Connection.RemoteIpAddress?.ToString() ??
                        "desconhecido";

                    return RateLimitPartition.GetFixedWindowLimiter(identificador, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = configuration.GetValue<int>("LimiteRequisicoes:EndpointPorMinuto"),
                        Window = TimeSpan.FromMinutes(1)
                    });
                });

                // Código de rejeição (429)
                opcoes.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // Gatilho para registrar penalidade
                opcoes.OnRejected = async (contexto, token) =>
                {
                    var servicoPenalidade = contexto.HttpContext.RequestServices.GetRequiredService<IUserPenaltyService>();

                    var identificador =
                        contexto.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                        contexto.HttpContext.Connection.RemoteIpAddress?.ToString();

                    await servicoPenalidade.RegistrarStrikeAsync(identificador, token);

                    await contexto.HttpContext.Response.WriteAsync(
                        "Muitas requisições. Tente novamente mais tarde.",
                        token);
                };
            });

            return services;
        }
    }
}
