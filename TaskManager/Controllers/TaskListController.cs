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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await taskListService.CreateAsync(dto);
        return Ok();
    }

    [HttpPut("{taskListId:int}")]
    public async Task<IActionResult> Update(int taskListId, [FromBody] UpdateTaskListModelDto model, [FromQuery] int userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (taskListId != model.Id)
            return BadRequest();

        var isUpdated = await taskListService.UpdateAsync(userId, model);

        return isUpdated ? Ok() : StatusCode(403);
    }

    [HttpGet("{taskListId:int}")]
    public async Task<IActionResult> GetById(int taskListId, [FromQuery] int userId)
    {
        var result = await taskListService.FindByIdAsync(userId, taskListId);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await taskListService.GetOwnedOrShared(userId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{taskListId:int}/shares")]
    public async Task<IActionResult> GetShares(int taskListId, [FromQuery] int userId)
    {
        var result = await taskListService.FindSharedUsersAsync(userId, taskListId);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpDelete("{taskListId:int}")]
    public async Task<IActionResult> Delete(int taskListId, [FromQuery] int userId)
    {
        var isDeleted = await taskListService.DeleteAsync(userId, taskListId);
        return isDeleted ? Ok() : StatusCode(403);
    }
}