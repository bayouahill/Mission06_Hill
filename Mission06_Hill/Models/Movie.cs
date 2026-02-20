using System.ComponentModel.DataAnnotations;

namespace Mission06_Hill.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year is required.")]
        [Range(1888, 2100, ErrorMessage = "Year must be 1888 or later.")]
        public int Year { get; set; }

        public string? Director { get; set; }

        public string? Rating { get; set; }

        [Required]
        public bool Edited { get; set; }

        public string? LentTo { get; set; }

        [Required]
        public bool CopiedToPlex { get; set; }

        [MaxLength(25)]
        public string? Notes { get; set; }

        // Navigation property
        public Category Category { get; set; }
    }
}
