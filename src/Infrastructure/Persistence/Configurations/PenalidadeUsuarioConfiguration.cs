using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PenalidadeUsuarioConfiguration : IEntityTypeConfiguration<PenalidadeUsuario>
    {
        public void Configure(EntityTypeBuilder<PenalidadeUsuario> builder)
        {
            builder.HasKey(p => p.Id);

            // impede a duplicação de strikes para o mesmo usuário/IP
            builder.HasIndex(p => p.Identificador)
                   .IsUnique();
        }
    }
}
