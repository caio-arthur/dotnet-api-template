using Application.Common.Models;

namespace Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public Erro Error { get; }

        public ValidationException(Erro error) : base(error.Mensagem)
        {
            Error = error;
        }
    }
}