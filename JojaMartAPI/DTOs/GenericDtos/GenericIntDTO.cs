using System.ComponentModel.DataAnnotations;

namespace JojaMartAPI.DTOs.GenericDtos
{
	public class GenericIntDTO
	{
		[Required]
		public int IntValue { get; set; }
	}
}
