using System.Security.Claims;

namespace Workhub.Application.Interfaces.JWT;

public interface IJWTGenerator
{
    string GenerateJWTToken(IList<Claim> claims);
}
