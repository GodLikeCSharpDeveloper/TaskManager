using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Entities;

namespace TaskManager.Configurations;

public class TaskListConfiguration : IEntityTypeConfiguration<TaskListModel>
{
    public void Configure(EntityTypeBuilder<TaskListModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.OwnerId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasMany(x => x.Shares)
            .WithOne(x => x.TaskList)
            .HasForeignKey(x => x.TaskListId);
    }
}