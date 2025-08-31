using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class CreateTaskListShareDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int OwnerUserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TaskListId { get; set; }
    }
}