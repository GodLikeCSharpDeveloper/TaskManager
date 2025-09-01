using TaskManager.Models;

namespace TaskManager.Services.TaskListShare
{
    public interface ITaskListShareService
    {
        Task<bool> CreateAsync(int ownerId, CreateTaskListShareDto createTaskListShare);
        Task<bool> DeleteAsync(int ownerId, int taskListId);
    }
}