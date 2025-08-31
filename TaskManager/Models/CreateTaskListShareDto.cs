using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class CreateTaskListShareDto
    {
        [Required]
        public int TaskListId { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public int OwnerUserId { get; set; }
    }
}