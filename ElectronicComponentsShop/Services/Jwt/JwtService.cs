using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Jwt
{
    public class JwtService : IJwtService
    {

        private readonly IConfiguration _config;
        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public string GetToken(UserDTO user)
        {
            //header
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:SecurityKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //payload
            var claims = GetClaims(user);
            var payload = new JwtPayload(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, DateTime.Now, DateTime.Now.AddMinutes(30));

            //jwt string
            var jst = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(jst);
        }
        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private IEnumerable<Claim> GetClaims(UserDTO user)
        {
            var userName = $"{user.LastName} {user.FirstName}";
            var claims = new List<Claim>
            {
                new Claim("Id",user.Id.ToString()),
                new Claim("Name", userName),
            };
            foreach (var value in user.Roles)
                claims.Add(new Claim("Role", value));

            return claims;
        }

        public IEnumerable<Claim> GetUserClaims(string tokenString)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
            return token.Claims;
        }
    }
}
