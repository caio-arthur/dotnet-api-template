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

        public DbSet<RegistroAuditoria> RegistrosAuditoria { get; set; }
        public DbSet<MensagemOutbox> MensagensOutbox => Set<MensagemOutbox>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessarEventosOutbox();

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ProcessarEventosOutbox()
        {
            var eventosDeDominio = ChangeTracker
                .Entries<EntidadeBase>()
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

            var mensagensOutbox = eventosDeDominio.Select(evento => new MensagemOutbox
            {
                DataCriacao = DateTime.Now,
                Tipo = evento.GetType().AssemblyQualifiedName!,
                Conteudo = JsonSerializer.Serialize(evento, evento.GetType())
            }).ToList();

            MensagensOutbox.AddRange(mensagensOutbox);
        }
    }
}