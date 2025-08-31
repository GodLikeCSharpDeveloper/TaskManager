using TaskManager.Repositories;

namespace TaskManager.Entities
{
    public class TaskListModel : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<TaskListShareModel> Shares { get; set; } = [];
    }
}