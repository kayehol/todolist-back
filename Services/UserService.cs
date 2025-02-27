using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using todo_back.Models;

namespace todo_back.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> ListUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUser(User user)
    {
        return await _context.Users.SingleOrDefaultAsync(
                u => u.Login == user.Login
                );
    }

    public bool UserExists(string login)
    {
        return _context.Users.Any(u => u.Login == login);
    }

    public async Task<User> CreateUser(User newUser)
    {
        newUser.Password = new PasswordHasher<User>().HashPassword(newUser, newUser.Password);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }
}
