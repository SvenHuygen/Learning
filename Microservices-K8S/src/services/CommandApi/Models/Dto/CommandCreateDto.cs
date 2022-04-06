using System.ComponentModel.DataAnnotations;

namespace CommandApi.Models.Dto
{
    public class CommandCreateDto
    {
        [Required]
        public string Man { get; set; }

        [Required]
        public string CommandLineName { get; set; }
    }
}