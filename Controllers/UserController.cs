using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo_back.Models;

namespace todo_back.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ApplicationDbContext _context;

    public UserController(ILogger<UserController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    async public Task<ActionResult<IEnumerable<User>>> ListUsers()
    {
        return await _context.Users.ToListAsync();
    }

    [HttpPost]
    async public Task<ActionResult<IEnumerable<User>>> CreateUser(User newUser)
    {
        newUser.Password = new PasswordHasher<User>().HashPassword(newUser, newUser.Password);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateUser), new { id = newUser.Id }, newUser);
    }
}
