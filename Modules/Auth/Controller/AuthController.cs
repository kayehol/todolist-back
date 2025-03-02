using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using todo_back.Models;
using todo_back.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;

    public AuthController(
            UserService userService,
            AuthService authService
    )
    {
        _userService = userService;
        _authService = authService;
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

        PasswordVerificationResult result = _authService.VerifyPassword(user, userLogin);

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Credenciais inválidas");

        string token = _authService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }



}
