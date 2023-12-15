namespace JojaMartAPI.DTOs.JwtDtos
{
    public class AuthenticatedUserResponse
    {
        public string AccessJwt { get; set; }
        public string RefreshJwt { get; set; }
    }
}
