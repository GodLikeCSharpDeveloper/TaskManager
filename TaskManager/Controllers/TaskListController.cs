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
    public async Task<IActionResult> Update([FromQuery] int ownerId, int taskListId, [FromBody] UpdateTaskListModelDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isUpdated = await taskListService.UpdateAsync(ownerId, model, taskListId);

        return isUpdated ? Ok() : StatusCode(403);
    }

    [HttpGet("{taskListId:int}")]
    public async Task<IActionResult> GetById(int taskListId, [FromQuery] int ownerId)
    {
        var result = await taskListService.FindByIdAsync(ownerId, taskListId);
        return result != null ? Ok(result) : NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int ownerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (pageSize > 100 || pageSize <= 0 || page < 1)
            return BadRequest("Invalid paging parameters");

        var result = await taskListService.GetOwnedOrShared(ownerId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{taskListId:int}/shares")]
    public async Task<IActionResult> GetShares(int taskListId, [FromQuery] int ownerId)
    {
        var result = await taskListService.FindSharedUsersAsync(ownerId, taskListId);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpDelete("{taskListId:int}")]
    public async Task<IActionResult> Delete(int taskListId, [FromQuery] int ownerId)
    {
        var isDeleted = await taskListService.DeleteAsync(ownerId, taskListId);
        return isDeleted ? Ok() : StatusCode(403);
    }
}