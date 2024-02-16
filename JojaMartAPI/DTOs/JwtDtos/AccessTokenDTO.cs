using System.ComponentModel.DataAnnotations;

namespace JojaMartAPI.DTOs.JwtDtos
{
    public class AccessTokenDTO
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
