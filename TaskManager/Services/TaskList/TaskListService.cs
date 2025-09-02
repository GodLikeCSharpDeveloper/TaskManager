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

        public async Task CreateAsync(CreateTaskListDto createTaskListDto)
        {
            var model = _mapper.Map<TaskListModel>(createTaskListDto);
            await _repo.CreateAsync(model);
        }

        public async Task<bool> UpdateAsync(int ownerId, UpdateTaskListModelDto taskListModel, int taskListId)
        {
            var hasAccess = await HasAccessAsync(ownerId, taskListId);
            if (!hasAccess)
                return false;

            var entity = await _repo.FindByIdWithSharesAsync(taskListId);
            if (entity == null)
                return false;

            if (!string.IsNullOrWhiteSpace(taskListModel.Name))
                entity.Name = taskListModel.Name;

            if (taskListModel.OwnerId.HasValue)
                entity.OwnerId = taskListModel.OwnerId.Value;

            if (taskListModel.SharedUserIds?.Count > 0)
            {
                var currentUserIds = entity.Shares.Select(s => s.UserId).ToHashSet();
                var newUserIds = taskListModel.SharedUserIds
                    .Where(id => id != ownerId)
                    .ToHashSet();

                entity.Shares.RemoveAll(s => !newUserIds.Contains(s.UserId));

                var toAdd = newUserIds.Except(currentUserIds);
                foreach (var userId in toAdd)
                {
                    entity.Shares.Add(new TaskListShareModel
                    {
                        TaskListId = taskListId,
                        UserId = userId
                    });
                }
            }

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int ownerId, int taskListId)
        {
            var isOwner = await IsOwnerAsync(ownerId, taskListId);
            if (isOwner)
                await _repo.DeleteAsync(taskListId);

            return isOwner;
        }

        public async Task<FindTaskListWithSharesDto?> FindByIdAsync(int ownerId, int taskListId)
        {
            if (await HasAccessAsync(ownerId, taskListId))
            {
                var dbObject = await _repo.FindByIdWithSharesAsync(taskListId);
                return _mapper.Map<FindTaskListWithSharesDto>(dbObject);
            }

            return null;
        }

        public async Task<List<FindTaskListWithSharesDto>> GetOwnedOrShared(int ownerId, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            var models = await _repo
                .AsQueryable()
                .Include(t => t.Shares)
                .Where(task =>
                    task.OwnerId == ownerId ||
                    task.Shares.Any(share => share.UserId == ownerId))
                .OrderByDescending(task => task.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return _mapper.Map<List<FindTaskListWithSharesDto>>(models);
        }

        public async Task<FindTaskListSharedUsersDto?> FindSharedUsersAsync(int ownerId, int taskListId)
        {
            if (await HasAccessAsync(ownerId, taskListId))
            {
                return await _repo.AsQueryable()
                    .Include(s => s.Shares)
                    .Where(s => s.Id == taskListId)
                    .Select(s => new FindTaskListSharedUsersDto { OwnerUserId = s.OwnerId, SharedUserIds = s.Shares.Select(share => share.UserId).ToList(), TaskListId = taskListId })
                    .FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<bool> HasAccessAsync(int ownerId, int taskListId)
        {
            if (ownerId == 0 || taskListId == 0)
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

            return taskList.OwnerId == ownerId || taskList.Shares.Contains(ownerId);
        }

        public async Task<bool> IsOwnerAsync(int ownerId, int taskListId)
        {
            if (ownerId == 0 || taskListId == 0)
                return false;

            return await _repo.AsQueryable()
                .AnyAsync(t => t.Id == taskListId && t.OwnerId == ownerId);
        }
    }
}