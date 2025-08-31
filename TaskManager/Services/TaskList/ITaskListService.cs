using TaskManager.Models;

namespace TaskManager.Services.TaskList
{
    public interface ITaskListService
    {
        Task CreateAsync(CreateTaskListDto createTaskListDto);
        Task<bool> UpdateAsync(int ownerId, UpdateTaskListModelDto taskListModel);
        Task<bool> DeleteAsync(int userId, int taskListId);
        Task<FindTaskListWithSharesDto?> FindByIdAsync(int userId, int taskListId);
        Task<List<FindTaskListWithSharesDto>> GetOwnedOrShared(int userId, int page, int pageSize);
        Task<FindTaskListSharedUsersDto?> FindSharedUsersAsync(int userId, int taskListId);
        Task<bool> HasAccessAsync(int userId, int taskListId);
        Task<bool> IsOwnerAsync(int userId, int taskListId);
    }
}