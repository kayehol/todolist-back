using Microsoft.AspNetCore.Mvc;
using todo_back.Models;
using todo_back.Services;

namespace todo_back.Controllers;

[ApiController]
[Route("api/tasks")]
public class TodoTaskController : ControllerBase
{
    private readonly ILogger<TodoTaskController> _logger;
    private readonly TodoTaskService _todoTaskService;

    public TodoTaskController(ILogger<TodoTaskController> logger, TodoTaskService todoTaskService)
    {
        _logger = logger;
        _todoTaskService = todoTaskService;
    }

    [HttpGet]
    public async Task<IEnumerable<TodoTask>> ListTodoTasks()
    {
        return await _todoTaskService.ListTodoTasks();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTask>> GetTodoTask(int id)
    {
        TodoTask? todoTask = await _todoTaskService.GetTodoTask(id);

        if (todoTask == null)
            return NotFound();

        return todoTask;
    }

    [HttpPost]
    public async Task<ActionResult<TodoTask>> CreateTodoTask(TodoTask newTask)
    {
        await _todoTaskService.CreateTodoTask(newTask);

        return CreatedAtAction(nameof(CreateTodoTask), new { id = newTask.Id }, newTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodoTask(int id, TodoTask todoTask)
    {
        if (id != todoTask.Id)
            return BadRequest();

        await _todoTaskService.UpdateTodoTask(todoTask);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveTodoTask(int id)
    {
        TodoTask? todoTask = await _todoTaskService.GetTodoTask(id);

        if (todoTask == null)
            return NotFound();

        await _todoTaskService.RemoveTodoTask(todoTask);

        return NoContent();
    }
}
