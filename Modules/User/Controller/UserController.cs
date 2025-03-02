using Microsoft.AspNetCore.Mvc;
using todo_back.Services;
using todo_back.Models;
using Microsoft.AspNetCore.Authorization;

namespace todo_back.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserService _userService;

    public UserController(ILogger<UserController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    /// <summary>
    /// Lista usuários
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<User>> ListUsers()
    {
        return await _userService.ListUsers();
    }
}
