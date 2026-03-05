using Domain.Common.Interfaces;

namespace Domain.Common.Primitives
{
    public abstract class EntidadeAuditavel : IAuditableEntity
    {
        public DateTime CriadoEm { get; set; }
        public Guid? CriadoPor { get; set; }
        public DateTime? AtualizadoEm { get; set; }
        public Guid? AtualizadoPor { get; set; }
    }
}
