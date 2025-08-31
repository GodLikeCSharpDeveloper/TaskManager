using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services.TaskList;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/task-lists")]
public class TaskListsController(ITaskListService taskListService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskListDto dto)
    {
        await taskListService.CreateAsync(dto);
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskListModelDto model, [FromQuery] int userId)
    {
        if (id != model.Id)
            return BadRequest();

        var isUpdated = await taskListService.UpdateAsync(userId, model);

        return isUpdated ? Ok() : StatusCode(403);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, [FromQuery] int userId)
    {
        var result = await taskListService.FindByIdAsync(userId, id);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int userId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var result = await taskListService.GetOwnedOrShared(userId, skip, take);
        return Ok(result);
    }

    [HttpGet("{id:int}/shares")]
    public async Task<IActionResult> GetShares(int id, [FromQuery] int userId)
    {
        var result = await taskListService.FindSharedUsersAsync(userId, id);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
    {
        var isDeleted = await taskListService.DeleteAsync(userId, id);
        return isDeleted ? Ok() : StatusCode(403);
    }
}