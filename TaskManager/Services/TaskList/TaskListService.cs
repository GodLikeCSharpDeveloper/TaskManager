using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Entities;
using TaskManager.Models;
using TaskManager.Repositories.TaskList;

namespace TaskManager.Services.TaskList
{
    public class TaskListService(ITaskListRepository taskListRepository, IMapper mapper) : ITaskListService
    {
        private readonly ITaskListRepository _repo = taskListRepository;
        private readonly IMapper _mapper = mapper;

        public Task CreateAsync(CreateTaskListDto createTaskListDto)
        {
            var model = _mapper.Map<TaskListModel>(createTaskListDto);
            return _repo.CreateAsync(model);
        }

        public async Task<bool> UpdateAsync(int ownerId, UpdateTaskListModelDto taskListModel)
        {
            var hasAccess = await HasAccessAsync(ownerId, taskListModel.Id);
            if (!hasAccess)
                return false;

            var entity = await _repo.GetByIdAsync(taskListModel.Id);
            if (entity is null)
                return false;

            if (!string.IsNullOrWhiteSpace(taskListModel.Name))
                entity.Name = taskListModel.Name;

            if (taskListModel.OwnerId != 0)
                entity.OwnerId = taskListModel.OwnerId;

            if (taskListModel.Shares.Count > 0)
                entity.Shares = taskListModel.Shares
                    .Select(s => new TaskListShareModel
                    {
                        TaskListId = taskListModel.Id,
                        UserId = s.UserId
                    })
                    .ToList();

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int userId, int taskListId)
        {
            var isOwner = await IsOwnerAsync(userId, taskListId);
            if (isOwner)
                await _repo.DeleteAsync(taskListId);

            return isOwner;
        }

        public async Task<FindTaskListWithSharesDto?> FindByIdAsync(int userId, int taskListId)
        {
            if (await HasAccessAsync(userId, taskListId))
            {
                var dbObject = await _repo.FindByIdWithSharesAsync(taskListId);
                return _mapper.Map<FindTaskListWithSharesDto>(dbObject);
            }

            return null;
        }

        public async Task<List<FindTaskListWithSharesDto>> GetOwnedOrShared(int userId, int skip, int take)
        {
            var models = await _repo
                .AsQueryable()
                .Include(t => t.Shares)
                .Where(task =>
                    task.OwnerId == userId ||
                    task.Shares.Any(share => share.UserId == userId))
                .OrderByDescending(task => task.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return _mapper.Map<List<FindTaskListWithSharesDto>>(models);
        }

        public async Task<FindTaskListSharedUsersDto?> FindSharedUsersAsync(int userId, int taskListId)
        {
            if (await HasAccessAsync(userId, taskListId))
            {
                return await _repo.AsQueryable()
                    .Include(s => s.Shares)
                    .Where(s => s.Id == taskListId)
                    .Select(s => new FindTaskListSharedUsersDto { OwnerUserId = s.OwnerId, SharedUserIds = s.Shares.Select(share => share.UserId).ToList(), TaskListId = taskListId })
                    .FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> HasAccessAsync(int userId, int taskListId)
        {
            if (userId == 0 || taskListId == 0)
                return false;

            var taskList = await _repo.AsQueryable()
                .Where(t => t.Id == taskListId)
                .Select(t => new
                {
                    t.OwnerId,
                    Shares = t.Shares.Select(s => s.UserId)
                })
                .FirstOrDefaultAsync();

            if (taskList == null)
                return false;

            return taskList.OwnerId == userId || taskList.Shares.Contains(userId);
        }

        public async Task<bool> IsOwnerAsync(int userId, int taskListId)
        {
            if (userId == 0 || taskListId == 0)
                return false;

            return await _repo.AsQueryable()
                .AnyAsync(t => t.Id == taskListId && t.OwnerId == userId);
        }
    }
}