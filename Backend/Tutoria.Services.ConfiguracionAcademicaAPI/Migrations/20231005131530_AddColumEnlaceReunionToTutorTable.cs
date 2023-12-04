using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddColumEnlaceReunionToTutorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnlaceReunion",
                table: "tutores",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnlaceReunion",
                table: "tutores");
        }
    }
}
