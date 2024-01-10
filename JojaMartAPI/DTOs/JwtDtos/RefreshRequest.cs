using System.ComponentModel.DataAnnotations;

namespace JojaMartAPI.DTOs.JwtDtos
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
