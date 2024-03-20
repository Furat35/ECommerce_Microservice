using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.API.Services.Contracts
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateToken(List<Claim> authClaims);
    }
}
