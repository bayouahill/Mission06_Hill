using System.ComponentModel.DataAnnotations;

namespace Mission06_Hill.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        [Required]
        public string RatingName { get; set; } = string.Empty;
        public ICollection<Movie> Movies { get; set; }
    }
}
