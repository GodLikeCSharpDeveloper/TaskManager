using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services.TaskListShare;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/task-list-shares")]
public class TaskListShareController(ITaskListShareService taskListShareService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] int ownerId, [FromBody] CreateTaskListShareDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isCreated = await taskListShareService.CreateAsync(ownerId, dto);
        return isCreated ? Ok() : StatusCode(403);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int ownerId, [FromQuery] int taskListId)
    {
        var isDeleted = await taskListShareService.DeleteAsync(ownerId, taskListId);
        return isDeleted ? Ok() : StatusCode(403);
    }
}