using Newtonsoft.Json;
using System.Collections.Generic;

namespace FakeAPI_PDSI.Model
{
    public class TokenResultModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public TokenResultModel()
        {
            Status = "F";
            Message = "Invalid Username";
            Token = "";
        }
    }

    public class LoginModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("RememberMe")]
        public int RememberMe { get; set; }
        [JsonProperty("method")]
        public string method { get; set; }

    }

    public class JWTProperty
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
    public class TokenModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }

   

    public class AuthenticationResult : TokenModel
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }


    public class ServiceConfiguration
    {
        public JwtSettings JwtSettings { get; set; }
    }
    public class JwtSettings
    {
        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }
    }

    public class UsersMaster
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
