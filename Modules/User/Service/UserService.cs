using Microsoft.EntityFrameworkCore;
using todo_back.Models;

namespace todo_back.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly AuthService _authService;

    public UserService(
            ApplicationDbContext context,
            AuthService authService
    )
    {
        _context = context;
        _authService = authService;
    }

    public async Task<IEnumerable<User>> ListUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUser(string userLogin)
    {
        return await _context.Users.SingleOrDefaultAsync(
                u => u.Login == userLogin
                );
    }

    public bool UserExists(string login)
    {
        return _context.Users.Any(u => u.Login == login);
    }

    public async Task<User> CreateUser(User newUser)
    {
        newUser.Password = _authService.HashPassword(newUser);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }
}
