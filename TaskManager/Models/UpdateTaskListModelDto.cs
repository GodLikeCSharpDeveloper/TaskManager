namespace TaskManager.Models
{
    public class UpdateTaskListModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int OwnerId { get; set; }
        public List<CreateTaskListShareDto> Shares { get; set; } = [];
    }
}