using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class UpdateTaskListModelDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        public string Name { get; set; } = default!;
        public int OwnerId { get; set; }
        public List<CreateTaskListShareDto> Shares { get; set; } = [];
    }
}