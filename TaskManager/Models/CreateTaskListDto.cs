using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class CreateTaskListDto
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; } = default!;

        [Required]
        public int OwnerId { get; set; }
    }
}