using Microsoft.EntityFrameworkCore;
using todo_back.Models;

namespace todo_back.Services;

public class TodoTaskService
{
    private readonly ApplicationDbContext _context;

    public TodoTaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoTask>> ListTodoTasks()
    {
        return await _context.TodoTasks.ToListAsync();
    }

    public async Task<TodoTask?> GetTodoTask(int id)
    {
        return await _context.TodoTasks.FindAsync(id);
    }

    public bool taskExists(int id)
    {
        return _context.TodoTasks.Any(t => t.Id == id);
    }

    public async Task<TodoTask> CreateTodoTask(TodoTask newTodoTask)
    {
        _context.TodoTasks.Add(newTodoTask);
        await _context.SaveChangesAsync();

        return newTodoTask;
    }

    public async Task UpdateTodoTask(TodoTask todoTask)
    {
        _context.Entry(todoTask).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task RemoveTodoTask(TodoTask todoTask)
    {
        _context.TodoTasks.Remove(todoTask);
        await _context.SaveChangesAsync();
    }

}
