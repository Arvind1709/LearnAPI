using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnAPI.Utilities
{
    public class JwtHelper
    {
        //private const string Key = "this_is_a_super_secret_key_for_jwt"; // Replace with your secret key
        private const string Key = "mhplcjKDBdJlSjVlInp3z5kIj2WV0a2u0X1oNHL1XTg="; // Replace with your secret key
        private const string Issuer = "https://localhost:7005";
        private const string Audience = "https://localhost:7005";

        public static string GenerateToken(string username)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var creds = new SigningCredentials(key1, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
