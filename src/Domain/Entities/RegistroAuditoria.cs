using Domain.Enums;

namespace Domain.Entities
{
    public class RegistroAuditoria
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual string Entidade { get; set; } = string.Empty;
        public virtual AuditoriaAcao Acao { get; set; } = AuditoriaAcao.NaoEspecificada;
        public virtual string ChavePrimaria { get; set; }
        public virtual string ValoresAntigos { get; set; } // JSON
        public virtual string ValoresNovos { get; set; } // JSON
        public virtual DateTime DataHora { get; set; } = DateTime.Now;
        public virtual Guid? UsuarioId { get; set; }
    }
}
