namespace PlatformApi.Models.MessageBus
{
    public class PlatformPublishDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Event { get; set; }
    }
}