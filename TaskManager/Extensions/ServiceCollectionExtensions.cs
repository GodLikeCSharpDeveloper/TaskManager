using TaskManager.Configurations;
using TaskManager.Repositories;
using TaskManager.Repositories.TaskList;
using TaskManager.Repositories.TaskListShare;
using TaskManager.Services.Retry;
using TaskManager.Services.TaskList;
using TaskManager.Services.TaskListShare;

namespace TaskManager.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTaskManagerDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITaskListRepository, TaskListRepository>();
            services.AddScoped<ITaskListShareRepository, TaskListShareRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(EfRepository<>));

            services.AddHostedService<AppDbMigrationHandler>();

            services.AddScoped<ITaskListService, TaskListService>();
            services.AddScoped<ITaskListShareService, TaskListShareService>();

            services.AddSingleton<IRetryService, RetryService>();
            return services;
        }
    }
}