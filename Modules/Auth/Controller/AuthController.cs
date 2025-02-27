using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

using todo_back.Models;
using todo_back.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserService _userService;

    public AuthController(
        IConfiguration configuration,
        UserService userService
    )
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (_userService.UserExists(user.Login))
            return BadRequest("Usuário já existe");

        await _userService.CreateUser(user);

        return Ok("Usuário criado com sucesso");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User userLogin)
    {
        User? user = await _userService.GetUser(userLogin);

        if (user == null)
            return Unauthorized("Credenciais inválidas");

        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, userLogin.Password);

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Credenciais inválidas");

        var token = GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
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


}
