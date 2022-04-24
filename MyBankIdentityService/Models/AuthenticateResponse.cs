using System.Text.Json.Serialization;

namespace MyBankIdentityService.Models
{
    public class AuthenticateResponse
    {
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public AuthenticateResponse()
        {
        }

        public AuthenticateResponse(string jwtToken, string refreshToken)
        {
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
