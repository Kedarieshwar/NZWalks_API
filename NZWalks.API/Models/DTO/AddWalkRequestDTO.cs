using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequestDTO
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Walk name must be at most 100 characters long")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Walk description must be at most 1000 characters long")]
        public string Description { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Walk length must be a positive number between 0 and 100")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
