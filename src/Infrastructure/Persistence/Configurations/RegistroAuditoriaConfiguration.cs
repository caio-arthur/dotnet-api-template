using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RegistroAuditoriaConfiguration : IEntityTypeConfiguration<RegistroAuditoria>
    {
        public void Configure(EntityTypeBuilder<RegistroAuditoria> builder)
        {
            builder.HasKey(ra => ra.Id);

        }
    }
}
