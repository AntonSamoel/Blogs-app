using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogs.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class makeCategoryIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Categories_CategoryID",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Post",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Categories_CategoryID",
                table: "Post",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Categories_CategoryID",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Categories_CategoryID",
                table: "Post",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
