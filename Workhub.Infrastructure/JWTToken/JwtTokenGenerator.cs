using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Workhub.Application.Interfaces.JWT;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.JWTToken;

public sealed class JwtTokenGenerator : IJWTGenerator
{
    private readonly IConfiguration configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    string IJWTGenerator.GenerateJWTToken(GlobalUser profile)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]));
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, profile.Email),
            new Claim(ClaimTypes.NameIdentifier, profile.Id)
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds);

        //var tokenDescriptor = new SecurityTokenDescriptor
        //{
        //    Subject = new ClaimsIdentity(new[]{
        //          new Claim(ClaimTypes.Email, profile.Email),
        //          new Claim(ClaimTypes.NameIdentifier, profile.Id)
        //    }),
        //    Expires = DateTime.UtcNow.AddDays(30),
        //    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        //};
        return new JwtSecurityTokenHandler().WriteToken(token);

        //var token = tokenHandler.CreateToken(tokenDescriptor);
        //return tokenHandler.WriteToken(token);
    }
}

