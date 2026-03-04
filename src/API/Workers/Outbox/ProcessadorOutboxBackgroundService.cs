using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Workers.Outbox
{
    public class ProcessadorOutboxBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessadorOutboxBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Roda a cada 10 segundos
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                // Como é um Singleton (BackgroundService), precisamos de um Scope para usar Scoped Services (DbContext, MediatR)
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                // Busca as mensagens não processadas (Pode limitar com .Take(20) para não travar)
                var mensagens = await dbContext.MensagensOutbox
                    .Where(m => m.DataProcessamento == null)
                    .OrderBy(m => m.DataCriacao)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var mensagem in mensagens)
                {
                    try
                    {
                        // Deserializa de volta para o evento original
                        var tipoEvento = Type.GetType(mensagem.Tipo);
                        if (tipoEvento == null) continue;

                        var evento = JsonSerializer.Deserialize(mensagem.Conteudo, tipoEvento);
                        if (evento is not INotification notificacao) continue;

                        // Dispara o evento
                        await publisher.Publish(notificacao, stoppingToken);

                        // Marca como processado
                        mensagem.DataProcessamento = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        // Fire and forget focado na estabilidade: salva o log e segue a vida
                        mensagem.Erro = ex.Message;
                    }
                }

                if (mensagens.Any())
                {
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
        }
    }
}
