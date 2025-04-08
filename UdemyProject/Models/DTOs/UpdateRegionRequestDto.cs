using System.ComponentModel.DataAnnotations;

namespace UdemyProject.Models.DTOs
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = " Min of 3")]
        [MaxLength(3, ErrorMessage = "Mx of 3")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Max can be 100")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
