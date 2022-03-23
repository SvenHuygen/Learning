namespace CommandApi.Models.Dto
{
    public class CommandReadDto
    {
        public Guid Id { get; set; }

        public string Man { get; set; }

        public string CommandLineName { get; set; }

        public Guid PlatformId { get; set; }
    }
}