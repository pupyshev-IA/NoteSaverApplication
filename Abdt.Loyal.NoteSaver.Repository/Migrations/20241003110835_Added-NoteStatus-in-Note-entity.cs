using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abdt.Loyal.NoteSaver.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedNoteStatusinNoteentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Notes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notes");
        }
    }
}
