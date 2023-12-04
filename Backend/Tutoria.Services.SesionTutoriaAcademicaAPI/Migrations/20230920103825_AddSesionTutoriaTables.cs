using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSesionTutoriaTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "programacionesReservaObligatoria",
                columns: table => new
                {
                    IdProgramacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTutor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicioSesionTutoria = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programacionesReservaObligatoria", x => x.IdProgramacion);
                });

            migrationBuilder.CreateTable(
                name: "tutorHorariosDisponible",
                columns: table => new
                {
                    IdHorario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTutor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    Dia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutorHorariosDisponible", x => x.IdHorario);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    IdReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCargaTutoria = table.Column<int>(type: "int", nullable: false),
                    IdProgramacionReservaObligatoria = table.Column<int>(type: "int", nullable: true),
                    IdTutorHorarioDisponible = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraTutoria = table.Column<TimeSpan>(type: "time", nullable: false),
                    TipoReunion = table.Column<int>(type: "int", nullable: false),
                    TipoReserva = table.Column<int>(type: "int", nullable: false),
                    EstadoConfirmacion = table.Column<int>(type: "int", nullable: false),
                    EnlaceReunion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LugarReunion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK_reservas_programacionesReservaObligatoria_IdProgramacionReservaObligatoria",
                        column: x => x.IdProgramacionReservaObligatoria,
                        principalTable: "programacionesReservaObligatoria",
                        principalColumn: "IdProgramacion");
                    table.ForeignKey(
                        name: "FK_reservas_tutorHorariosDisponible_IdTutorHorarioDisponible",
                        column: x => x.IdTutorHorarioDisponible,
                        principalTable: "tutorHorariosDisponible",
                        principalColumn: "IdHorario");
                });

            migrationBuilder.CreateTable(
                name: "sesionesTutoria",
                columns: table => new
                {
                    IdSesionTutoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdReserva = table.Column<int>(type: "int", nullable: false),
                    IdTutorado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdTutor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaReunion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sesionesTutoria", x => x.IdSesionTutoria);
                    table.ForeignKey(
                        name: "FK_sesionesTutoria_reservas_IdReserva",
                        column: x => x.IdReserva,
                        principalTable: "reservas",
                        principalColumn: "IdReserva",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detallesSesionTutoria",
                columns: table => new
                {
                    IdDetalleSesionTutoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSesionTutoria = table.Column<int>(type: "int", nullable: false),
                    Dimension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detallesSesionTutoria", x => x.IdDetalleSesionTutoria);
                    table.ForeignKey(
                        name: "FK_detallesSesionTutoria_sesionesTutoria_IdSesionTutoria",
                        column: x => x.IdSesionTutoria,
                        principalTable: "sesionesTutoria",
                        principalColumn: "IdSesionTutoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_detallesSesionTutoria_IdSesionTutoria",
                table: "detallesSesionTutoria",
                column: "IdSesionTutoria");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_IdProgramacionReservaObligatoria",
                table: "reservas",
                column: "IdProgramacionReservaObligatoria");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_IdTutorHorarioDisponible",
                table: "reservas",
                column: "IdTutorHorarioDisponible");

            migrationBuilder.CreateIndex(
                name: "IX_sesionesTutoria_IdReserva",
                table: "sesionesTutoria",
                column: "IdReserva");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detallesSesionTutoria");

            migrationBuilder.DropTable(
                name: "sesionesTutoria");

            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "programacionesReservaObligatoria");

            migrationBuilder.DropTable(
                name: "tutorHorariosDisponible");
        }
    }
}
