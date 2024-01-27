namespace Workhub.Application.Interfaces.JWT;

public interface IJWTGenerator
{
    string GenerateJWTToken(string email, string userid);
}
