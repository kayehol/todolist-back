using System.ComponentModel.DataAnnotations;

namespace todo_back.Models;

public class User
{
    public User(string login, string password)
    {
        Login = login;
        Password = password;
        CreatedAt = DateTime.Now;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }

    public DateTime CreatedAt { get; set; }
}
