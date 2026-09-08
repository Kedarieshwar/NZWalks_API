using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Region code must be at least 3 characters long")]
        [MaxLength(3, ErrorMessage = "Region code must be at most 3 characters long")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Region name must be at most 100 characters long")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
