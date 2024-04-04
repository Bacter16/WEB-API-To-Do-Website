using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class numaivreau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Users_UserId",
                table: "TodoTasks");

            migrationBuilder.DropIndex(
                name: "IX_TodoTasks_UserId",
                table: "TodoTasks");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_UserId",
                table: "TodoTasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Users_UserId",
                table: "TodoTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Users_UserId",
                table: "TodoTasks");

            migrationBuilder.DropIndex(
                name: "IX_TodoTasks_UserId",
                table: "TodoTasks");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_UserId",
                table: "TodoTasks",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Users_UserId",
                table: "TodoTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
