using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_back.Models;
using todo_back.Services;

namespace todo_back.Controllers;

[Authorize]
[ApiController]
[Route("api/tasks")]
[Produces("application/json")]
public class TodoTaskController : ControllerBase
{
    private readonly ILogger<TodoTaskController> _logger;
    private readonly TodoTaskService _todoTaskService;

    public TodoTaskController(ILogger<TodoTaskController> logger, TodoTaskService todoTaskService)
    {
        _logger = logger;
        _todoTaskService = todoTaskService;
    }

    /// <summary>
    /// Lista tarefas
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<TodoTask>> ListTodoTasks()
    {
        return await _todoTaskService.ListTodoTasks();
    }

    /// <summary>
    /// Lista tarefas paginadas
    /// </summary>
    /// <returns></returns>
    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListPaginated(int page = 1, int pageSize = 10)
    {
        string? userLogin = User.FindFirst(ClaimTypes.Name)?.Value;

        var result = await _todoTaskService.ListPaginated(page, pageSize, userLogin!);
        return Ok(result);
    }

    /// <summary>
    /// Busca uma tarefa
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTask>> GetTodoTask(int id)
    {
        TodoTask? todoTask = await _todoTaskService.GetTodoTask(id);

        if (todoTask == null)
            return NotFound();

        return todoTask;
    }

    /// <summary>
    /// Cria uma tarefa
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<TodoTask>> CreateTodoTask(TodoTask newTask)
    {
        await _todoTaskService.CreateTodoTask(newTask);

        return CreatedAtAction(nameof(CreateTodoTask), new { id = newTask.Id }, newTask);
    }

    /// <summary>
    /// Atualiza uma tarefa
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTodoTask(int id, [FromBody] TodoTask todoTask)
    {
        if (id != todoTask.Id)
            return BadRequest();

        bool exists = _todoTaskService.TaskExists(id);

        if (!exists)
            return NotFound();

        await _todoTaskService.UpdateTodoTask(todoTask);

        return NoContent();
    }

    /// <summary>
    /// Remove uma tarefa
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveTodoTask(int id)
    {
        TodoTask? todoTask = await _todoTaskService.GetTodoTask(id);

        if (todoTask == null)
            return NotFound();


        await _todoTaskService.RemoveTodoTask(todoTask);

        return NoContent();
    }
}
