using System.ComponentModel.DataAnnotations;

namespace CommandApi.Models
{
    public class Platform
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Command> Commands { get; set; } = new List <Command>();
    }
}