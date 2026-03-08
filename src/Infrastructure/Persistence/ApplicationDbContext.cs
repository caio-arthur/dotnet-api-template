using Application.Common.Interfaces;
using Domain.Common.Primitives;
using Domain.Entities;
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

        public DbSet<AuditoriaRegistro> AuditoriaRegistros { get; set; }
        public DbSet<OutboxMensagens> OutboxMensagens => Set<OutboxMensagens>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessarEventosOutbox();

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (Database.IsNpgsql()) ConfigurePostgres(modelBuilder);
            modelBuilder.HasDefaultSchema("dominio");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static void ConfigurePostgres(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCollation(
                name: "case_insensitive",
                locale: "und-u-ks-level1",
                provider: "icu",
                deterministic: false);
        }

        private void ProcessarEventosOutbox()
        {
            var eventosDeDominio = ChangeTracker
                .Entries<EntidadeComEventos>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var eventos = entity.EventosDeDominio.ToList();
                    entity.LimparEventosDeDominio();
                    return eventos;
                })
                .ToList();

            if (!eventosDeDominio.Any())
                return;

            var mensagensOutbox = eventosDeDominio.Select(evento => new OutboxMensagens
            {
                DataCriacao = DateTime.Now,
                Tipo = evento.GetType().AssemblyQualifiedName!,
                Conteudo = JsonSerializer.Serialize(evento, evento.GetType())
            }).ToList();

            OutboxMensagens.AddRange(mensagensOutbox);
        }

    }
}