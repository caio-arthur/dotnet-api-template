using Application.Common.Models;

namespace Application.Common.Exceptions
{
    public class DomainException : Exception
    {
        public Erro Error { get; }

        public DomainException(Erro error) : base(error.Mensagem)
        {
            Error = error;
        }
    }
}