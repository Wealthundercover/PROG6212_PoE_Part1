using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty; // e.g., "Organiser", "Participant"

        // Navigation property
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}