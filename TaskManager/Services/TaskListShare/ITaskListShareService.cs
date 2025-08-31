using TaskManager.Models;

namespace TaskManager.Services.TaskListShare
{
    public interface ITaskListShareService
    {
        Task<bool> CreateAsync(CreateTaskListShareDto createTaskListShare);
        Task<bool> DeleteAsync(int userId, int taskListId);
    }
}