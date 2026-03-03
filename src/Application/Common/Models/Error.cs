namespace Application.Common.Models
{
    public class Error
    {
        public Error(int codigo, string nome, string mensagem)
        {
            Codigo = codigo;
            Nome = nome;
            Mensagem = mensagem;
        }

        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Mensagem { get; set; }

        public static Error Default => InternalServerError;

        /// <summary>
        /// HTTP Response Codes
        /// </summary>
        public static Error InternalServerError => new Error(500, nameof(InternalServerError), "Ocorreu um erro inesperado no servidor.");
        public static Error Validation => new Error(400, nameof(Validation), "Requisição mal formatada.");
        public static Error BadRequest => new Error(400, nameof(BadRequest), "A requisição é inválida.");
        public static Error Unauthorized => new Error(401, nameof(Unauthorized), "Falha na autenticação do usuário.");
        public static Error Forbidden => new Error(403, nameof(Forbidden), "O usuário não tem permissão para acessar o recurso.");
        public static Error NotFound => new Error(404, nameof(NotFound), "O recurso solicitado não foi encontrado.");
        public static Error MethodNotAllowed => new Error(405, nameof(MethodNotAllowed), "O método HTTP utilizado não é permitido para o recurso.");
        public static Error RequestTimeout => new Error(408, nameof(RequestTimeout), "O servidor demorou muito para receber a requisição.");
        public static Error Conflict => new Error(409, nameof(Conflict), "Conflito ao tentar criar ou modificar um recurso.");
        public static Error UnsupportedMediaType => new Error(415, nameof(UnsupportedMediaType), "O tipo de mídia enviado na requisição não é suportado.");
        public static Error TooManyRequests => new Error(429, nameof(TooManyRequests), "O cliente fez muitas requisições em um curto período (rate limit).");
        public static Error BadGateway => new Error(502, nameof(BadGateway), "O servidor recebeu uma resposta inválida de um serviço upstream.");
        public static Error ServiceUnavailable => new Error(503, nameof(ServiceUnavailable), "O serviço está temporariamente indisponível.");
        public static Error GatewayTimeout => new Error(504, nameof(GatewayTimeout), "O servidor não recebeu uma resposta a tempo de um serviço upstream.");
    }
}
