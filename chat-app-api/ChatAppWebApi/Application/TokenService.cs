using Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SHG.Application;

public record AccessTokenDto(string? Issuer, string? Audience, double Expires, string Token);

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public TokenService(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<AccessTokenDto> GenerateJwtToken(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
                     {
                         new(JwtRegisteredClaimNames.Sub, user.UserName!),
                         new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                         new(JwtRegisteredClaimNames.Email, user.Email!),
                         new("username", user.UserName!),
                     };

        claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"]));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AccessTokenDto(tokenDescriptor.Issuer, tokenDescriptor.Audience,
                                  Convert.ToDouble(_configuration["JwtSettings:AccessTokenExpirationMinutes"]) * 60, tokenHandler.WriteToken(token));
    }
}
