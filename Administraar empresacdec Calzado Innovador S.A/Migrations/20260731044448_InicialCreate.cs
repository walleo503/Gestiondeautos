using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Migrations
{
    /// <inheritdoc />
    public partial class InicialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenesProduccion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroOrden = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Producto = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CantidadAProducir = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaEntregaEstimada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesProduccion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcesosFabricacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    DuracionEstimadaHoras = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesosFabricacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdenProcesos",
                columns: table => new
                {
                    OrdenProduccionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcesoFabricacionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Completado = table.Column<bool>(type: "INTEGER", nullable: false),
                    FechaCompletado = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Secuencia = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenProcesos", x => new { x.OrdenProduccionId, x.ProcesoFabricacionId });
                    table.ForeignKey(
                        name: "FK_OrdenProcesos_OrdenesProduccion_OrdenProduccionId",
                        column: x => x.OrdenProduccionId,
                        principalTable: "OrdenesProduccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenProcesos_ProcesosFabricacion_ProcesoFabricacionId",
                        column: x => x.ProcesoFabricacionId,
                        principalTable: "ProcesosFabricacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesProduccion_NumeroOrden",
                table: "OrdenesProduccion",
                column: "NumeroOrden",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdenProcesos_ProcesoFabricacionId",
                table: "OrdenProcesos",
                column: "ProcesoFabricacionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcesosFabricacion_Nombre",
                table: "ProcesosFabricacion",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenProcesos");

            migrationBuilder.DropTable(
                name: "OrdenesProduccion");

            migrationBuilder.DropTable(
                name: "ProcesosFabricacion");
        }
    }
}
