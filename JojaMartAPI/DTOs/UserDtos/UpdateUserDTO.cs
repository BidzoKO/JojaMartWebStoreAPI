using System.ComponentModel.DataAnnotations;

namespace JojaMartAPI.DTOs.UserDtos
{
	public class UpdateUserDTO
	{
		[Required]
		public string? UserFullName { get; set; }
		[Required]
		public string? UserName { get; set; }
		[Required]
		public string? Address { get; set; }
		[Required]
		public int UserPhone { get; set; }
		[Required]
		public string? ImageUrl { get; set; }
		[Required]
		public string? UserPassword { get; set; }
	}
}
