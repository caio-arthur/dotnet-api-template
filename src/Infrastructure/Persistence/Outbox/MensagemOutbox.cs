namespace Infrastructure.Persistence.Outbox
{
    public class MensagemOutbox
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public string? Erro { get; set; }
    }
}
