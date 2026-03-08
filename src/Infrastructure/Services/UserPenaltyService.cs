using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class UserPenaltyService : IUserPenaltyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserPenaltyService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> IsBanidoAsync(string identificador, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(identificador))
                return false;

            var penalidade = await _context.PenalidadeUsuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Identificador == identificador, token);

            if (penalidade is null)
                return false;

            // Verifica banimento permanente
            if (penalidade.BanidoPermanentemente) return true;

            // Verifica se está no período de bloqueio temporário
            if (penalidade.BloqueadoAte.HasValue && penalidade.BloqueadoAte.Value > DateTime.Now) return true;

            return false;
        }

        public async Task RegistrarStrikeAsync(string identificador, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(identificador))
                return;

            var penalidade = await _context.PenalidadeUsuarios
                .FirstOrDefaultAsync(p => p.Identificador == identificador, token);

            // Se não existe, cria o primeiro registro
            if (penalidade == null)
            {
                penalidade = new PenalidadeUsuario
                {
                    Identificador = identificador,
                    QuantidadeStrikes = 1,
                    DataUltimoStrike = DateTime.Now
                };
                await _context.PenalidadeUsuarios.AddAsync(penalidade, token);
            }
            else
            {
                // Se já existe, incrementa
                penalidade.QuantidadeStrikes++;
                penalidade.DataUltimoStrike = DateTime.Now;

                // Aplica as regras de negócio baseadas na configuração
                AplicarRegrasDePunicao(penalidade);
            }

            await _context.SaveChangesAsync(token);
        }

        private void AplicarRegrasDePunicao(PenalidadeUsuario penalidade)
        {
            // Lendo as configurações (você pode injetar via IOptions no futuro para ficar mais elegante)
            var limiteStrikesTemporario = _configuration.GetValue<int>("Penalidades:StrikesParaBloqueioTemporario", 3);
            var tempoBloqueioMinutos = _configuration.GetValue<int>("Penalidades:TempoBloqueioMinutos", 15);
            var limiteStrikesPermanente = _configuration.GetValue<int>("Penalidades:StrikesParaBanimento", 10);

            if (penalidade.QuantidadeStrikes >= limiteStrikesPermanente)
            {
                penalidade.BanidoPermanentemente = true;
                penalidade.BloqueadoAte = null; // Já está permanente, não precisa de data final
            }
            else if (penalidade.QuantidadeStrikes % limiteStrikesTemporario == 0)
            {
                // A cada X strikes (ex: 3, 6, 9), aplica um bloqueio temporário
                penalidade.BloqueadoAte = DateTime.Now.AddMinutes(tempoBloqueioMinutos);
            }
        }
    }
}