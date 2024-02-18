using System.ComponentModel.DataAnnotations;

namespace JojaMartAPI.DTOs.GenericDtos
{
	public class GenericStringDTO
	{
		[Required]
		public string StringValue { get; set; } = null!;
	}
}
