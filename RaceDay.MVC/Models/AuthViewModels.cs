using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
	public class LoginViewModel
	{
		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required, DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
	}

	public class RegisterViewModel
	{
		[Required]
		public string FullName { get; set; } = string.Empty;

		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required, DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required]
		public string Role { get; set; } = "Participant"; // "Organiser" or "Participant"
	}

	public class AuthResponseDto
	{
		public string Token { get; set; } = string.Empty;
		public int UserId { get; set; }
		public string Role { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
	}
}