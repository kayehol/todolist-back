using Microsoft.AspNetCore.Mvc;
using todo_back.Services;
using todo_back.Models;

namespace todo_back.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserService _userService;

    public UserController(ILogger<UserController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> ListUsers()
    {
        return await _userService.ListUsers();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        User? user = await _userService.GetUser(id);

        if (user == null)
            return NotFound();

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User newUser)
    {
        await _userService.CreateUser(newUser);

        return CreatedAtAction(nameof(CreateUser), new { id = newUser.Id }, newUser);
    }
}
