using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLA.UserFlow.Application.DTO;
using Microsoft.IdentityModel.Tokens;

namespace BLA.UserFlow.Application.Services.TokenHandler;

public class TokenHandler : ITokenHandler
{
    public string CreateJWTToken(UserByEmailDto user)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(ClaimTypes.Name, user.FirstName));
        claims.Add(new Claim(ClaimTypes.GivenName, user.LastName ?? ""));
        claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTKEY") ?? ""));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            Environment.GetEnvironmentVariable("JWTISSUER"),
            Environment.GetEnvironmentVariable("JWTAUDIENCE"),
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}