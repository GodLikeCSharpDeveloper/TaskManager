namespace TaskManager.Models
{
    public class FindTaskListWithSharesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<int> SharedUserIds { get; set; } = [];
    }
}
