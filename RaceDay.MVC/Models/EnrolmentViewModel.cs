namespace RaceDay.MVC.Models
{
    public class EnrolmentViewModel
    {
        public int EnrolmentId { get; set; }
        public int UserId { get; set; }
        public UserViewModel? User { get; set; }
        public int EventId { get; set; }
        public EventViewModel? Event { get; set; }
        public int? CategoryId { get; set; }
        public CategoryViewModel? Category { get; set; }
        public DateTime EnrolmentDate { get; set; }
    }

    public class UserViewModel
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }

    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}