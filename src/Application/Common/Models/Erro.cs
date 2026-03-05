namespace Application.Common.Models
{
    public class Erro
    {
        public Erro(int codigo, string nome, string mensagem)
        {
            Codigo = codigo;
            Nome = nome;
            Mensagem = mensagem;
        }

        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Mensagem { get; set; }

        public static Erro Default => InternalServerError;

        /// <summary>
        /// HTTP Response Codes
        /// </summary>
        public static Erro InternalServerError => new Erro(500, nameof(InternalServerError), "Ocorreu um erro inesperado no servidor.");
        public static Erro Validation => new Erro(400, nameof(Validation), "Requisição mal formatada.");
        public static Erro BadRequest => new Erro(400, nameof(BadRequest), "A requisição é inválida.");
        public static Erro Unauthorized => new Erro(401, nameof(Unauthorized), "Falha na autenticação do usuário.");
        public static Erro Forbidden => new Erro(403, nameof(Forbidden), "O usuário não tem permissão para acessar o recurso.");
        public static Erro NotFound => new Erro(404, nameof(NotFound), "O recurso solicitado não foi encontrado.");
        public static Erro MethodNotAllowed => new Erro(405, nameof(MethodNotAllowed), "O método HTTP utilizado não é permitido para o recurso.");
        public static Erro RequestTimeout => new Erro(408, nameof(RequestTimeout), "O servidor demorou muito para receber a requisição.");
        public static Erro Conflict => new Erro(409, nameof(Conflict), "Conflito ao tentar criar ou modificar um recurso.");
        public static Erro UnsupportedMediaType => new Erro(415, nameof(UnsupportedMediaType), "O tipo de mídia enviado na requisição não é suportado.");
        public static Erro TooManyRequests => new Erro(429, nameof(TooManyRequests), "O cliente fez muitas requisições em um curto período (rate limit).");
        public static Erro BadGateway => new Erro(502, nameof(BadGateway), "O servidor recebeu uma resposta inválida de um serviço upstream.");
        public static Erro ServiceUnavailable => new Erro(503, nameof(ServiceUnavailable), "O serviço está temporariamente indisponível.");
        public static Erro GatewayTimeout => new Erro(504, nameof(GatewayTimeout), "O servidor não recebeu uma resposta a tempo de um serviço upstream.");

        // Genericos
        public static Erro FalhaDesserializacao => new Erro(500, nameof(FalhaDesserializacao), "Falha ao desserializar os dados.");


        // Auditoria
        public static Erro AuditoriaAcaoInvalida => new Erro(400, nameof(AuditoriaAcaoInvalida), "Apenas registros com ação de Exclusão podem ser recuperados por este comando.");
        public static Erro AuditoriaSemDadosAntigos => new Erro(500, nameof(AuditoriaSemDadosAntigos), "Não há dados antigos salvos para recuperar esta entidade.");
        public static Erro AuditoriaEntidadeNaoMapeada => new Erro(500, nameof(AuditoriaEntidadeNaoMapeada), "Tipo de entidade não mapeado no DbContext.");
    }
}
