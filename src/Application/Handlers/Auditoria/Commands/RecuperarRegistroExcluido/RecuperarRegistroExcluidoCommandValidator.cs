using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Auditoria.Commands.RecuperarRegistroExcluido
{
    public class RecuperarRegistroExcluidoCommandValidator : AbstractValidator<RecuperarRegistroExcluidoCommand>
    {
        private readonly IApplicationDbContext _context;

        public RecuperarRegistroExcluidoCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Id)
                .NotEmpty();

            RuleFor(x => x).CustomAsync(async (request, context, ct) =>
            {
                var auditoria = await _context.AuditoriaRegistros
                    .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

                if (auditoria == null)
                {
                    context.AddFailure(new ValidationFailure(nameof(request.Id), "Registro de auditoria não encontrado.") { CustomState = Erro.NotFound });
                    return;
                }

                if (auditoria.Acao != AuditoriaAcao.Exclusao)
                {
                    context.AddFailure(new ValidationFailure(nameof(request.Id), "Ação inválida.") { CustomState = Erro.AuditoriaAcaoInvalida });
                    return;
                }

                if (string.IsNullOrEmpty(auditoria.ValoresAntigos))
                {
                    context.AddFailure(new ValidationFailure(nameof(request.Id), "Dados antigos não encontrados.") { CustomState = Erro.AuditoriaSemDadosAntigos });
                    return;
                }

                var entityType = _context.Model.GetEntityTypes()
                    .FirstOrDefault(t => t.ClrType.Name == auditoria.Entidade);

                if (entityType == null)
                {
                    context.AddFailure(new ValidationFailure(nameof(request.Id), "Entidade não mapeada.") { CustomState = Erro.AuditoriaEntidadeNaoMapeada });
                    return;
                }
            });
        }
    }
}
