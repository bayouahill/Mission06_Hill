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
        public string Category { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int Year { get; set; }
        [Required]
        public string Director { get; set; } = string.Empty;
        [Required]
        public string Rating { get; set; } // this stores selected value
        public bool Edited { get; set; } = false;
        public string? LentTo { get; set; }
        [MaxLength(25)]
        public string? Notes { get; set; }

        [NotMapped] // tells EF to ignore this property
        public SelectList RatingOptions= new SelectList(new[]
        {
            "G","PG","PG-13","R"
        });
    }
}
