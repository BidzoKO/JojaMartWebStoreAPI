using System.ComponentModel.DataAnnotations;

namespace JojaMartAPI.DTOs.UserDtos
{
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
