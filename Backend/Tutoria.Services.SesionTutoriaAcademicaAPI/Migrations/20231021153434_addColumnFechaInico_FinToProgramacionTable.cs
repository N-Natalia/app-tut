using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class addColumnFechaInico_FinToProgramacionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaOficial",
                table: "programacionesReservaObligatoria",
                newName: "FechaInicio");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "programacionesReservaObligatoria",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "programacionesReservaObligatoria");

            migrationBuilder.RenameColumn(
                name: "FechaInicio",
                table: "programacionesReservaObligatoria",
                newName: "FechaOficial");
        }
    }
}
