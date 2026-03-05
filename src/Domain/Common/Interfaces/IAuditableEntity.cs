namespace Domain.Common.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CriadoEm { get; set; }
        Guid? CriadoPor { get; set; }
        DateTime? AtualizadoEm { get; set; }
        Guid? AtualizadoPor { get; set; }
    }
}
