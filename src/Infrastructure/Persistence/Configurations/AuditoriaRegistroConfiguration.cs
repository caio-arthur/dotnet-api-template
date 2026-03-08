using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configurations
{
    public class AuditoriaRegistroConfiguration : IEntityTypeConfiguration<AuditoriaRegistro>
    {
        public void Configure(EntityTypeBuilder<AuditoriaRegistro> builder)
        {
            builder.HasKey(ra => ra.Id);

            builder.Property(x => x.ValoresAntigos)
                .HasColumnType("jsonb");
            
            builder.Property(x => x.ValoresNovos)
                .HasColumnType("jsonb");

        }
    }
}
