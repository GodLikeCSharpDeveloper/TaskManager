using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Entities;
using TaskManager.Models;
using TaskManager.Repositories.TaskListShare;
using TaskManager.Services.TaskList;

namespace TaskManager.Services.TaskListShare
{
    public class TaskListShareService(ITaskListShareRepository taskListShareRepository, ITaskListService taskListService, IMapper mapper) : ITaskListShareService
    {
        private readonly ITaskListShareRepository _repo = taskListShareRepository;
        private readonly ITaskListService _taskListService = taskListService;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreateAsync(int ownerId, CreateTaskListShareDto createTaskListShare)
        {
            if (createTaskListShare.UserId == ownerId)
                return false;

            var hasAccess = await _taskListService.HasAccessAsync(ownerId, createTaskListShare.TaskListId);
            if (!hasAccess)
                return false;

            var model = _mapper.Map<TaskListShareModel>(createTaskListShare);

            await _repo.CreateAsync(model);
            return true;
        }

        public async Task<bool> DeleteAsync(int ownerId, int taskListId)
        {
            var hasAccess = await _taskListService.HasAccessAsync(ownerId, taskListId);
            if (hasAccess)
            {
                await _repo.AsQueryable()
                    .Where(x => x.TaskListId == taskListId && x.UserId == ownerId)
                    .ExecuteDeleteAsync();
            }

            return hasAccess;
        }
    }
}