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

        public async Task<bool> CreateAsync(CreateTaskListShareDto createTaskListShare)
        {
            if (createTaskListShare.UserId == createTaskListShare.OwnerUserId)
                return false;

            var hasAccess = await _taskListService.HasAccessAsync(createTaskListShare.OwnerUserId, createTaskListShare.TaskListId);
            if (!hasAccess)
                return false;

            var model = _mapper.Map<TaskListShareModel>(createTaskListShare);

            await _repo.CreateAsync(model);
            return true;
        }

        public async Task<bool> DeleteAsync(int userId, int taskListId)
        {
            var hasAccess = await _taskListService.HasAccessAsync(userId, taskListId);
            if (hasAccess)
            {
                await _repo.AsQueryable()
                    .Where(x => x.TaskListId == taskListId && x.UserId == userId)
                    .ExecuteDeleteAsync();
            }

            return hasAccess;
        }
    }
}