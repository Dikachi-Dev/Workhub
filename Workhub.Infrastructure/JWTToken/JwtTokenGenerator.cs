using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Interfaces.JWT;

namespace Workhub.Infrastructure.JWTToken;

public sealed class JwtTokenGenerator :IJWTGenerator
{
    private readonly IConfiguration configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    string IJWTGenerator.GenerateJWTToken(string email, string userid)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey( Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]{
                  new Claim(JwtRegisteredClaimNames.Email, email),
                  new Claim(JwtRegisteredClaimNames.UniqueName, userid)
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

