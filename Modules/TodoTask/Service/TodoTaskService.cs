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

    public async Task<PaginatedTodoTask<TodoTask>> ListPaginated(int pageNumber, int pageSize)
    {
        int total = await _context.TodoTasks.CountAsync();

        List<TodoTask> tasks = await _context.TodoTasks
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // return await _context.TodoTasks.ToListAsync();
        //
        return new PaginatedTodoTask<TodoTask>
        {
            Tasks = tasks,
            TotalCount = total,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
    }

    public async Task<TodoTask?> GetTodoTask(int id)
    {
        return await _context.TodoTasks.FindAsync(id);
    }

    public bool TaskExists(int id)
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
