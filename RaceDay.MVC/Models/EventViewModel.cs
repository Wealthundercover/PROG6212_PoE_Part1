using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
	public class EventViewModel
	{
		public int Id { get; set; }

		[Required, Display(Name = "Event Name")]
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		[Required, DataType(DataType.Date), Display(Name = "Event Date")]
		public DateTime Date { get; set; }

		public string Location { get; set; } = string.Empty;

		[Display(Name = "Max Capacity")]
		public int MaxCapacity { get; set; }

		[Display(Name = "Enrolled Participants")]
		public int EnrolledCount { get; set; }

		public string Status { get; set; } = "Upcoming"; // "Upcoming", "InProgress", "Completed"

		public string? ImageUrl { get; set; } // Will connect to Azure Blob Storage
	}

	public class CreateEventViewModel
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		[Required, DataType(DataType.Date)]
		public DateTime Date { get; set; } = DateTime.Today.AddDays(7);

		[Required]
		public string Location { get; set; } = string.Empty;

		[Required, Range(1, 10000)]
		public int MaxCapacity { get; set; } = 100;

		public IFormFile? ImageFile { get; set; } // For Azure Blob upload via MVC
	}
}