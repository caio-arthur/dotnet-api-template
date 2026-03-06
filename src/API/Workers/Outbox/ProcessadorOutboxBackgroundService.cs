using Infrastructure.Persistence;
using Infrastructure.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace API.Workers.Outbox
{
    public class ProcessadorOutboxBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProcessadorOutboxBackgroundService> _logger;
        private readonly int _intervaloEmSegundos;
        public ProcessadorOutboxBackgroundService(
                IServiceProvider serviceProvider,
                IOptions<OutboxOptions> options,
                ILogger<ProcessadorOutboxBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _intervaloEmSegundos = options.Value.IntervaloEmSegundos > 0 ? options.Value.IntervaloEmSegundos : 10;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Background Service iniciado. Intervalo: {Intervalo}s", _intervaloEmSegundos);

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_intervaloEmSegundos));

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await ProcessarMensagensAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocorreu um erro fatal ao processar o Outbox.");
                }
            }
        }

        private async Task ProcessarMensagensAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

            var mensagens = await dbContext.OutboxMensagens
                .Where(m => m.DataProcessamento == null)
                .OrderBy(m => m.DataCriacao)
                .Take(20)
                .ToListAsync(stoppingToken);

            if (!mensagens.Any()) return;

            foreach (var mensagem in mensagens)
            {
                try
                {
                    var tipoEvento = Type.GetType(mensagem.Tipo);
                    if (tipoEvento == null)
                    {
                        mensagem.Erro = $"Tipo não encontrado: {mensagem.Tipo}";
                        continue;
                    }

                    var evento = JsonSerializer.Deserialize(mensagem.Conteudo, tipoEvento);
                    if (evento is INotification notificacao)
                    {
                        await publisher.Publish(notificacao, stoppingToken);
                        mensagem.DataProcessamento = DateTime.UtcNow;
                    }
                }
                catch (Exception ex)
                {
                    mensagem.Erro = ex.Message;
                    _logger.LogWarning(ex, "Erro ao processar mensagem do Outbox ID {Id}", mensagem.Id);
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}
