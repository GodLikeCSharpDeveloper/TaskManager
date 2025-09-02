using TaskManager.Models;

namespace TaskManager.Services.TaskList
{
    public interface ITaskListService
    {
        Task CreateAsync(CreateTaskListDto createTaskListDto);
        Task<bool> UpdateAsync(int ownerId, UpdateTaskListModelDto taskListModel, int taskListId);
        Task<bool> DeleteAsync(int ownerId, int taskListId);
        Task<FindTaskListWithSharesDto?> FindByIdAsync(int ownerId, int taskListId);
        Task<List<FindTaskListWithSharesDto>> GetOwnedOrShared(int ownerId, int page, int pageSize);
        Task<FindTaskListSharedUsersDto?> FindSharedUsersAsync(int ownerId, int taskListId);
        Task<bool> HasAccessAsync(int ownerId, int taskListId);
        Task<bool> IsOwnerAsync(int ownerId, int taskListId);
    }
}