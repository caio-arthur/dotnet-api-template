using Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MensagemOutboxConfiguration : IEntityTypeConfiguration<OutboxMensagens>
    {
        public void Configure(EntityTypeBuilder<OutboxMensagens> builder)
        {
            builder.HasKey(mo => mo.Id);
        }
    }
}
