using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Jwt
{
    public interface IJwtService
    {
        string GetToken(UserDTO user);
        string GetRefreshToken();
        IEnumerable<Claim> GetUserClaims(string tokenString);
    }
}
