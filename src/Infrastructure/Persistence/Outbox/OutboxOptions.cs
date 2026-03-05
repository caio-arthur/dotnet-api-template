namespace Infrastructure.Persistence.Outbox
{
    public class OutboxOptions
    {
        public const string SectionName = "Outbox";

        public bool Habilitado { get; set; } = true;
        public int IntervaloEmSegundos { get; set; } = 10;
    }
}
