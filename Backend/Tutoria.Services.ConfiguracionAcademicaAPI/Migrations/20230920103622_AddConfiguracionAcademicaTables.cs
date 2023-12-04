using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguracionAcademicaTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApPaterno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApMaterno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "semestres",
                columns: table => new
                {
                    IdSemestre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DenominacionSemestre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semestres", x => x.IdSemestre);
                });

            migrationBuilder.CreateTable(
                name: "administradores",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administradores", x => x.Code);
                    table.ForeignKey(
                        name: "FK_administradores_persons_Code",
                        column: x => x.Code,
                        principalTable: "persons",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tutorados",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NroCelular = table.Column<int>(type: "int", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonReferencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NroCelularPersonaReferencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutorados", x => x.Code);
                    table.ForeignKey(
                        name: "FK_tutorados_persons_Code",
                        column: x => x.Code,
                        principalTable: "persons",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tutores",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NroCelular = table.Column<int>(type: "int", nullable: false),
                    LugarReunion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutores", x => x.Code);
                    table.ForeignKey(
                        name: "FK_tutores_persons_Code",
                        column: x => x.Code,
                        principalTable: "persons",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cargasTutoria",
                columns: table => new
                {
                    IdCargaTutoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTutor = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdTutorado = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSemestre = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargasTutoria", x => x.IdCargaTutoria);
                    table.ForeignKey(
                        name: "FK_cargasTutoria_semestres_IdSemestre",
                        column: x => x.IdSemestre,
                        principalTable: "semestres",
                        principalColumn: "IdSemestre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cargasTutoria_tutorados_IdTutorado",
                        column: x => x.IdTutorado,
                        principalTable: "tutorados",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_cargasTutoria_tutores_IdTutor",
                        column: x => x.IdTutor,
                        principalTable: "tutores",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cargasTutoria_IdSemestre",
                table: "cargasTutoria",
                column: "IdSemestre");

            migrationBuilder.CreateIndex(
                name: "IX_cargasTutoria_IdTutor",
                table: "cargasTutoria",
                column: "IdTutor");

            migrationBuilder.CreateIndex(
                name: "IX_cargasTutoria_IdTutorado",
                table: "cargasTutoria",
                column: "IdTutorado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administradores");

            migrationBuilder.DropTable(
                name: "cargasTutoria");

            migrationBuilder.DropTable(
                name: "semestres");

            migrationBuilder.DropTable(
                name: "tutorados");

            migrationBuilder.DropTable(
                name: "tutores");

            migrationBuilder.DropTable(
                name: "persons");
        }
    }
}
