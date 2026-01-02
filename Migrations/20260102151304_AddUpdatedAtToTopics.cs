using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neksara.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAtToTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Topics",
                newName: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Topics",
                newName: "UpdateAt");
        }
    }
}
