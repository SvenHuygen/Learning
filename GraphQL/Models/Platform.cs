using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    public class Platform
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string LicenseKey { get; set; }
    }
}