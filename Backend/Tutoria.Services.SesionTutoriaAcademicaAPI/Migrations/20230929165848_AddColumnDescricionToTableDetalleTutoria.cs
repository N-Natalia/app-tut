using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnDescricionToTableDetalleTutoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Dimension",
                table: "detallesSesionTutoria",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "detallesSesionTutoria",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "detallesSesionTutoria");

            migrationBuilder.AlterColumn<string>(
                name: "Dimension",
                table: "detallesSesionTutoria",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
