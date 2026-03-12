using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace TaskFlowAPI.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;
        public JwtHelper(IConfiguration config)
        {
            _config=config;
        }
        public string GenerateToken(string username,string role)
        {
             var key = _config["JwtSettings:Key"] 
                      ?? throw new Exception("JWT key missing");
            var claims =new[]
            {
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.Role,role)
            };
           
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));       
            var creds=new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience:_config["JwtSettings:Audience"],
                claims:claims,
                expires:DateTime.Now.AddMinutes(
                    Convert.ToDouble(_config["JwtSettings:DurationInMinutes"])
                ),
                signingCredentials:creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}