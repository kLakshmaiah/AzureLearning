using System.ComponentModel.DataAnnotations;

namespace AzureLearning.Models
{
    public class AddContainer
    {
        [Required]
        public string? ContainerName { get; set; }

    }
}
