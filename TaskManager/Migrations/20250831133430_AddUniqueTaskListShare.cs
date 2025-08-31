using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueTaskListShare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskListShares_TaskListId",
                table: "TaskListShares");

            migrationBuilder.CreateIndex(
                name: "IX_TaskListShares_TaskListId_UserId",
                table: "TaskListShares",
                columns: new[] { "TaskListId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskListShares_TaskListId_UserId",
                table: "TaskListShares");

            migrationBuilder.CreateIndex(
                name: "IX_TaskListShares_TaskListId",
                table: "TaskListShares",
                column: "TaskListId");
        }
    }
}
