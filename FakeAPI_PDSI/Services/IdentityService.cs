using FakeAPI_PDSI.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using ConfigurationManager = FakeAPI_PDSI.Helper.ConfigurationManager;


namespace FakeAPI_PDSI.Services
{
    public interface IIdentityService
    {
        Task<ResponseModel> Login(LoginModel user);
    }
    public class IdentityService : IIdentityService
    {
        //private readonly DemoTokenContext _context;
        private readonly ServiceConfiguration _appSettings;

        private readonly TokenValidationParameters _tokenValidationParameters;
        public IdentityService()
        {
        }


        public async Task<ResponseModel> Login(LoginModel user)
        {
            ResponseModel result = new ResponseModel();
            if (user is null)
            {
                result.Message = "Invalid user request!!!";
            }

            if (user.UserName == "kairosdev" && user.Password == "password" && user.method == "Login")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"], audience: ConfigurationManager.AppSetting["JWT:ValidAudience"], claims: new List<Claim>(), expires: DateTime.Now.AddMinutes(600), signingCredentials: signinCredentials);
                //var tokeOptions = new JwtSecurityToken(issuer: jwt.ValidIssuer, audience: jwt.ValidAudience, claims: new List<Claim>(), expires: DateTime.Now.AddMinutes(600), signingCredentials: signinCredentials);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                
                result.Data = tokenString;
                result.IsSuccess = true;
                result.Message = "OK";
            }
            return result;
        }



    }
}
