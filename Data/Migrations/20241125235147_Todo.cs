using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ispit.Todo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Todo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TodoTasks_TodolistId",
                table: "TodoTasks");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TodolistId",
                table: "TodoTasks",
                column: "TodolistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TodoTasks_TodolistId",
                table: "TodoTasks");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TodolistId",
                table: "TodoTasks",
                column: "TodolistId",
                unique: true);
        }
    }
}
