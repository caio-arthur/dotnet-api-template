using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("penalidade_usuarios", Schema = "infra")]
    public class PenalidadeUsuario
    {
        [Column("id")]
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        [Column("identificador")]
        public virtual string Identificador { get; set; } = string.Empty; // Armazenará o ID do usuário (quando autenticado) ou o IP
        [Column("quantidade_strikes")]
        public virtual int QuantidadeStrikes { get; set; }
        [Column("bloqueado_ate")]
        public virtual DateTime? BloqueadoAte { get; set; }
        [Column("banido_permanentemente")]
        public virtual bool BanidoPermanentemente { get; set; }
        [Column("data_ultimo_strike")]
        public virtual DateTime DataUltimoStrike { get; set; }
    }
}
