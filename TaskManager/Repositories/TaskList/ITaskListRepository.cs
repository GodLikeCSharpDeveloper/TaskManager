using TaskManager.Entities;

namespace TaskManager.Repositories.TaskList
{
    public interface ITaskListRepository : IGenericRepository<TaskListModel>
    {
        Task<TaskListModel?> FindByIdWithSharesAsync(int id);
    }
}