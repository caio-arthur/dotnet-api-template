using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Handlers.Auditoria.Commands.RecuperarRegistroExcluido
{
    public class RecuperarRegistroExcluidoCommand : IRequestWrapper<RecuperarRegistroExcluidoResponse>
    {
        public Guid Id { get; set; }
    }

    public record RecuperarRegistroExcluidoResponse(string ChavePrimaria, string Mensagem);

    public class RecuperarRegistroExcluidoCommandHandler : IHandlerWrapper<RecuperarRegistroExcluidoCommand, RecuperarRegistroExcluidoResponse>
    {
        private readonly IApplicationDbContext _context; 

        public RecuperarRegistroExcluidoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resposta<RecuperarRegistroExcluidoResponse>> Handle(RecuperarRegistroExcluidoCommand request, CancellationToken cancellationToken)
        {
            var auditoria = await _context.AuditoriaRegistros
                .FirstAsync(a => a.Id == request.Id, cancellationToken);

            var entityType = _context.Model.GetEntityTypes()
                .First(t => t.ClrType.Name == auditoria.Entidade);

            var clrType = entityType.ClrType;

            try
            {
                var entidadeRecuperada = JsonSerializer.Deserialize(auditoria.ValoresAntigos!, clrType);

                if (entidadeRecuperada is null) return Resposta.Failure<RecuperarRegistroExcluidoResponse>(Erro.FalhaDesserializacao);

                _context.Add(entidadeRecuperada);

                await _context.SaveChangesAsync(cancellationToken);

                return Resposta.Success(new RecuperarRegistroExcluidoResponse(auditoria.ChavePrimaria, "Registro recuperado com sucesso!"));
            }
            catch (Exception ex)
            {
                return Resposta.Failure<RecuperarRegistroExcluidoResponse>(Erro.Default);
            }
        }
    }
}
