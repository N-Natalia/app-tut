using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDetallesProgramacionTableAndUdpdateProgramacionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraInicioSesionTutoria",
                table: "programacionesReservaObligatoria");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "programacionesReservaObligatoria",
                newName: "FechaOficial");

            migrationBuilder.AddColumn<int>(
                name: "TotalBloques",
                table: "programacionesReservaObligatoria",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "detallesProgramacionReservaObligatoria",
                columns: table => new
                {
                    IdDetalleProgramacionReservaObligatoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProgramacion = table.Column<int>(type: "int", nullable: false),
                    NroBloque = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicioSesionTutoria = table.Column<TimeSpan>(type: "time", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detallesProgramacionReservaObligatoria", x => x.IdDetalleProgramacionReservaObligatoria);
                    table.ForeignKey(
                        name: "FK_detallesProgramacionReservaObligatoria_programacionesReservaObligatoria_IdProgramacion",
                        column: x => x.IdProgramacion,
                        principalTable: "programacionesReservaObligatoria",
                        principalColumn: "IdProgramacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_detallesProgramacionReservaObligatoria_IdProgramacion",
                table: "detallesProgramacionReservaObligatoria",
                column: "IdProgramacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detallesProgramacionReservaObligatoria");

            migrationBuilder.DropColumn(
                name: "TotalBloques",
                table: "programacionesReservaObligatoria");

            migrationBuilder.RenameColumn(
                name: "FechaOficial",
                table: "programacionesReservaObligatoria",
                newName: "Fecha");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraInicioSesionTutoria",
                table: "programacionesReservaObligatoria",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
