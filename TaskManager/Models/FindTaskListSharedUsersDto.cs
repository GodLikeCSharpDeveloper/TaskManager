namespace TaskManager.Models
{
    public class FindTaskListSharedUsersDto
    {
        public int TaskListId { get; set; }
        public int OwnerUserId { get; set; }
        public List<int> SharedUserIds { get; set; } = [];
    }
}