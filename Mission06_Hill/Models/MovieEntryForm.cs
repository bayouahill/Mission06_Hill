using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mission06_Hill.Models
{
    public class MovieEntryForm
    {
        [Key]
        public int MovieId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Year { get; set; } = string.Empty; // string to support ranges like "2001-2002"
        public bool Edited { get; set; } = false;
        public string? LentTo { get; set; }
        [MaxLength(25)]
        public string? Notes { get; set; }

        // Foreign Keys
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int RatingId { get; set; }

        // Director name (text input - will be matched/created)
        [Required]
        public string DirectorName { get; set; } = string.Empty;

        // Dropdowns (not stored in DB)
        [NotMapped]
        public SelectList? CategoryOptions { get; set; }
        [NotMapped]
        public SelectList? RatingOptions { get; set; }
    }
}
