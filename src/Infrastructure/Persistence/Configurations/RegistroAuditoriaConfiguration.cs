using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RegistroAuditoriaConfiguration : IEntityTypeConfiguration<AuditoriaRegistro>
    {
        public void Configure(EntityTypeBuilder<AuditoriaRegistro> builder)
        {
            builder.HasKey(ra => ra.Id);
        }
    }
}
