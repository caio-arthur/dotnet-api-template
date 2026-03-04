using Application.Common.Interfaces;
using Domain.Common.Primitives;
using Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<MensagemOutbox> MensagensOutbox { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Pega todas as entidades que foram modificadas e possuem eventos
            var eventosDeDominio = ChangeTracker
                .Entries<EntidadeBase>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var eventos = entity.EventosDeDominio.ToList();
                    entity.LimparEventosDeDominio(); // Limpa para não disparar duas vezes
                    return eventos;
                })
                .ToList();

            // 2. Converte os eventos para a nossa entidade Outbox
            var mensagensOutbox = eventosDeDominio.Select(evento => new MensagemOutbox
            {
                Id = Guid.NewGuid(),
                DataCriacao = DateTime.Now,
                Tipo = evento.GetType().AssemblyQualifiedName!,
                Conteudo = JsonSerializer.Serialize(evento, evento.GetType())
            }).ToList();

            // 3. Adiciona as mensagens no contexto atual
            AddRange(mensagensOutbox);

            // 4. Salva tudo na mesma transação (Seus dados + Eventos na Fila)
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
