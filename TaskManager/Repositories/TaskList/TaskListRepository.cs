using Microsoft.EntityFrameworkCore;
using TaskManager.Configurations;
using TaskManager.Entities;

namespace TaskManager.Repositories.TaskList
{
    public class TaskListRepository(AppDbContext context) : EfRepository<TaskListModel>(context), ITaskListRepository
    {
        public Task<TaskListModel?> FindByIdWithSharesAsync(int id)
        {
            return AsQueryable().Include(tl => tl.Shares).FirstOrDefaultAsync(tl => tl.Id == id);
        }
    }
}