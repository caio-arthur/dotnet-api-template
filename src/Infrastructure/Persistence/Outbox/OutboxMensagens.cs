using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Persistence.Outbox
{
    [Table("outbox_mensagens", Schema = "infra")]
    public class OutboxMensagens
    {
        [Column("id")]
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        [Column("tipo")]
        public virtual string Tipo { get; set; } = string.Empty;
        [Column("conteudo")]
        public virtual string Conteudo { get; set; } = string.Empty;
        [Column("data_criacao")]
        public virtual DateTime DataCriacao { get; set; }
        [Column("data_processamento")]
        public virtual DateTime? DataProcessamento { get; set; }
        [Column("erro")]
        public virtual string Erro { get; set; }
    }
}
