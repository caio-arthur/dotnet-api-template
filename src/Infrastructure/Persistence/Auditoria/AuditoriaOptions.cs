namespace Infrastructure.Persistence.Auditoria
{
    public class AuditoriaOptions
    {
        public const string SectionName = "Auditoria";
        public bool Habilitado { get; set; } = true;
        public bool RegistrarHistoricoDetalhado { get; set; } = true; // Define se salva na tabela RegistroAuditoria
    }
}
