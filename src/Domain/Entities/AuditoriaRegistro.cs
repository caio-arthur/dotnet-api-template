using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("auditoria_registros", Schema = "infra")]
    public class AuditoriaRegistro
    {
        [Column("id")]
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        [Column("entidade")]
        public virtual string Entidade { get; set; } = string.Empty;
        [Column("acao")]
        public virtual AuditoriaAcao Acao { get; set; } = AuditoriaAcao.NaoEspecificada;
        [Column("chave_primaria")]
        public virtual string ChavePrimaria { get; set; }
        [Column("valores_antigos")]
        public virtual string ValoresAntigos { get; set; } // JSON
        [Column("valores_novos")]
        public virtual string ValoresNovos { get; set; } // JSON
        [Column("data_hora")]
        public virtual DateTime DataHora { get; set; } = DateTime.Now;
        [Column("usuario_id")]
        public virtual Guid? UsuarioId { get; set; }
    }
}
