using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.JWT;

public interface IJWTGenerator
{
    string GenerateJWTToken(GlobalUser profile);
}
