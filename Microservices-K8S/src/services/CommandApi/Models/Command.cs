using System.ComponentModel.DataAnnotations;

namespace CommandApi.Models
{
    public class Command
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Man { get; set; }

        [Required]
        public string CommandLineName { get; set; }

        [Required]
        public Guid PlatformId { get; set; }

        public Platform Platform { get; set; }
    }
}