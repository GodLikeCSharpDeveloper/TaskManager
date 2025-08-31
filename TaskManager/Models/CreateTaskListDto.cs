using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class CreateTaskListDto
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; } = default!;

        [Required]
        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }
    }
}