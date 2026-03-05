namespace Application.Common.Models
{
    public class Resposta 
    {
        public bool Sucesso { get; }
        public Erro Erro { get; }

        protected Resposta(bool success, Erro error)
        {
            Sucesso = success;
            Erro = error;
        }

        public static Resposta Failure(Erro error)
        {
            return new Resposta(false, error);
        }

        public static Resposta Success()
        {
            return new Resposta(true, null);
        }

        public static Resposta<TData> Failure<TData>(Erro error)
        {
            return new Resposta<TData>(false, default, error);
        }

        public static Resposta<TData> Success<TData>(TData value = default)
        {
            return new Resposta<TData>(true, value, null);
        }
    }

    public class Resposta<TData> : Resposta
    {
        public TData Dados { get; }

        public Resposta(bool isSuccess, TData data, Erro error) : base(isSuccess, error)
        {
            Dados = data;
        }
    }
}
