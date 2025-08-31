using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Entities;

namespace TaskManager.Configurations
{
    public class TaskListShareConfiguration : IEntityTypeConfiguration<TaskListShareModel>
    {
        public void Configure(EntityTypeBuilder<TaskListShareModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.TaskList)
                .WithMany(x => x.Shares)
                .HasForeignKey(x => x.TaskListId);

            builder
                .HasIndex(x => new { x.TaskListId, x.UserId })
                .IsUnique();
        }
    }
}