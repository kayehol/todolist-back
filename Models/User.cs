namespace todo_back.Models;

public class User
{
    public User(string login, string password)
    {
        Login = login;
        Password = password;
        CreatedAt = DateTime.Now;
    }

    public String Login { get; set; }

    public String Password { get; set; }

    public DateTime CreatedAt { get; set; }
}
