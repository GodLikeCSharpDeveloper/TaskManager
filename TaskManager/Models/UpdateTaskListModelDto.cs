using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class UpdateTaskListModelDto
    {
        [StringLength(255, MinimumLength = 1)]
        public string? Name { get; set; }

        [Range(1, int.MaxValue)]
        public int? OwnerId { get; set; }
        public List<int>? SharedUserIds { get; set; }
    }
}