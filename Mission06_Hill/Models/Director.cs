using System.ComponentModel.DataAnnotations;

namespace Mission06_Hill.Models
{
    public class Director
    {
        [Key]
        public int DirectorId { get; set; }
        [Required]
        public string DirectorName { get; set; } = string.Empty;
        public ICollection<Movie> Movies { get; set; }
    }
}
