using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using todo_back.Models;

namespace todo_back.Services;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
          new Claim(ClaimTypes.Name, user.Login),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Audience"],
          claims: claims,
          expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
          signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string HashPassword(User user)
    {
        return _passwordHasher.HashPassword(user, user.Password);
    }

    public PasswordVerificationResult VerifyPassword(User user, User userLogin)
    {
        return _passwordHasher.VerifyHashedPassword(user, user.Password, userLogin.Password);
    }

}
