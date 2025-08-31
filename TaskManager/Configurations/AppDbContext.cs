using Microsoft.EntityFrameworkCore;
using TaskManager.Entities;

namespace TaskManager.Configurations
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TaskListModel> TaskLists => Set<TaskListModel>();
        public DbSet<TaskListShareModel> TaskListShares => Set<TaskListShareModel>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskListConfiguration());
            modelBuilder.ApplyConfiguration(new TaskListShareConfiguration());
        }
    }
}