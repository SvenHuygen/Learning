namespace CommandApi.Models.MessageBus
{
    public class PlatformPublishedDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Event { get; set; }
    }
}