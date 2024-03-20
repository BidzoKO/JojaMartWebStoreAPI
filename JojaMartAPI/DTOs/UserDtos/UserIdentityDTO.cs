namespace JojaMartAPI.DTOs.UserDtos
{
	public class UserIdentityDTO
	{
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Username { get; set; } = null!;
		public string Email { get; set; } = null!;
		public DateTime Dob { get; set; }
		public string Gender { get; set; } = null!;
		public string? Address { get; set; }
		public string? PhoneNumber { get; set; }
		public string? CallingCode { get; set; }
		public string AccountStatus { get; set; } = null!;
		public string? ProfilePictureUrl { get; set; }



	}
}
