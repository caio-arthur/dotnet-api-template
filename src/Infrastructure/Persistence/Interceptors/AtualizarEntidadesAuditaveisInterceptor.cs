using Application.Common.Interfaces;
using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Persistence.Interceptors
{
    public class AtualizarEntidadesAuditaveisInterceptor : SaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AuditoriaOptions _options;

        public AtualizarEntidadesAuditaveisInterceptor(
            IServiceProvider serviceProvider,
            IOptions<AuditoriaOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext is null || !_options.Habilitado)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var currentUserService = _serviceProvider.GetService<ICurrentUserService>();
            var userId = currentUserService?.UserId ?? null;
            var dataAtual = DateTime.Now;

            var entries = dbContext.ChangeTracker.Entries<IAuditableEntity>().ToList();
            var registrosHistorico = new List<RegistroAuditoria>();

            foreach (var entry in entries)
            {
                // 1. Preenche as colunas da própria entidade
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CriadoPor = userId;
                    entry.Entity.CriadoEm = dataAtual;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.AtualizadoPor = userId;
                    entry.Entity.AtualizadoEm = dataAtual;
                }

                // 2. Cria o log detalhado na tabela separada (se habilitado)
                if (_options.RegistrarHistoricoDetalhado &&
                    (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted))
                {
                    var acaoAuditoria = entry.State switch
                    {
                        EntityState.Added => AuditoriaAcao.Insercao,
                        EntityState.Modified => AuditoriaAcao.Modificacao,
                        EntityState.Deleted => AuditoriaAcao.Exclusao,
                        _ => AuditoriaAcao.NaoEspecificada
                    };

                    var registro = new RegistroAuditoria
                    {
                        Entidade = entry.Metadata.ClrType.Name,
                        Acao = acaoAuditoria,
                        DataHora = dataAtual,
                        UsuarioId = userId,
                        ChavePrimaria = JsonSerializer.Serialize(
                            entry.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)
                        )
                    };

                    if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                    {
                        registro.ValoresAntigos = JsonSerializer.Serialize(
                            entry.Properties.Where(p => p.IsModified || entry.State == EntityState.Deleted)
                                            .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue)
                        );
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        registro.ValoresNovos = JsonSerializer.Serialize(
                            entry.Properties.Where(p => p.IsModified || entry.State == EntityState.Added)
                                            .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)
                        );
                    }

                    registrosHistorico.Add(registro);
                }
            }

            if (registrosHistorico.Any())
            {
                dbContext.Set<RegistroAuditoria>().AddRange(registrosHistorico);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
