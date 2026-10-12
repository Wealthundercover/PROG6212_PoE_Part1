using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 1000.0)]
        public decimal DistanceKm { get; set; }

        [Required]
        [MaxLength(50)]
        public string EventType { get; set; } = string.Empty; // e.g., "Run", "Walk", "Cycle"

        // Foreign Key for Organiser (User)
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? Organiser { get; set; }

        // Navigation properties
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}
