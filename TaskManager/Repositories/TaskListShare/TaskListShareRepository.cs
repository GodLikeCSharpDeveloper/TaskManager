using TaskManager.Configurations;
using TaskManager.Entities;

namespace TaskManager.Repositories.TaskListShare
{
    public class TaskListShareRepository(AppDbContext context) : EfRepository<TaskListShareModel>(context), ITaskListShareRepository
    {
    }
}