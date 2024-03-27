using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Workhub.Application.Interfaces.JWT;

namespace Workhub.Infrastructure.JWTToken;

public sealed class JwtTokenGenerator : IJWTGenerator
{
    private readonly IConfiguration configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    string IJWTGenerator.GenerateJWTToken(IList<Claim> claims)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

