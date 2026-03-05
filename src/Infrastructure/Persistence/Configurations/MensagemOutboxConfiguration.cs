using Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MensagemOutboxConfiguration : IEntityTypeConfiguration<MensagemOutbox>
    {
        public void Configure(EntityTypeBuilder<MensagemOutbox> builder)
        {
            builder.HasKey(mo => mo.Id);


        }
    }
}
