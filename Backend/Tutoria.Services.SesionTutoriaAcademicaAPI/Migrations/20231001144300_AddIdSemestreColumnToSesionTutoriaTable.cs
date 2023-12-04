using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIdSemestreColumnToSesionTutoriaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdSemestre",
                table: "sesionesTutoria",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdSemestre",
                table: "sesionesTutoria");
        }
    }
}
