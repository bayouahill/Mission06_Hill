using System.ComponentModel.DataAnnotations;

namespace Mission06_Hill.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        [Range(1900,3000)]
        public string Year { get; set; } = string.Empty;
        public bool Edited { get; set; } = false;
        public string? LentTo { get; set; }
        [MaxLength(25)]
        public string? Notes { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public int DirectorId { get; set; }
        public Director Director { get; set; }

        [Required]
        public int RatingId { get; set; }
        public Rating Rating { get; set; }
    }
}
