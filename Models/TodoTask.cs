using System.ComponentModel.DataAnnotations;

namespace todo_back.Models;

public class TodoTask
{
    public TodoTask(string title, string description, int userId)
    {
        Title = title;
        Description = description;
        UserId = userId;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    public bool Done { get; set; } = false;

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
