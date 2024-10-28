

namespace Ticket_Hub.Models.DTO.Auth
{
    public class SignByGoogleResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public Boolean IsProfileComplete { get; set; }
    }
}
