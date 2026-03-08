namespace Application.Common.Interfaces
{
    public interface IUserPenaltyService
    {
        // Método chamado no OnRejected do Rate Limiter
        Task RegistrarStrikeAsync(string identificador, CancellationToken token = default);

        // Método chamado no Middleware de verificação prévia
        Task<bool> IsBanidoAsync(string identificador, CancellationToken token = default);
    }
}
