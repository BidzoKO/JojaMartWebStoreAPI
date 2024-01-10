namespace JojaMartAPI.DTOs.JwtDtos
{
    public class AuthenticatedUserResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
