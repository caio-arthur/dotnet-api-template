namespace Infrastructure.Persistence.Outbox
{
    public class MensagemOutbox
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual string Tipo { get; set; } = string.Empty;
        public virtual string Conteudo { get; set; } = string.Empty;
        public virtual DateTime DataCriacao { get; set; }
        public virtual DateTime? DataProcessamento { get; set; }
        public virtual string Erro { get; set; }
    }
}
