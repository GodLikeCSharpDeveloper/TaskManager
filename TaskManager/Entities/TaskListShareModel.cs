using TaskManager.Repositories;

namespace TaskManager.Entities
{
    public class TaskListShareModel : IEntity
    {
        public int Id { get; set; }
        public int TaskListId { get; set; }
        public TaskListModel TaskList { get; set; } = default!;
        public int UserId { get; set; }
    }
}