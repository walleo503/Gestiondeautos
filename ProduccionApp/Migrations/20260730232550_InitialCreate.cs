using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProduccionApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenesProduccion",
                columns: table => new
                {
                    OrdenProduccionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Producto = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CantidadAProducir = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaEntregaEstimada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesProduccion", x => x.OrdenProduccionId);
                });

            migrationBuilder.CreateTable(
                name: "ProcesosFabricacion",
                columns: table => new
                {
                    ProcesoFabricacionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    DuracionEstimadaHoras = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesosFabricacion", x => x.ProcesoFabricacionId);
                });

            migrationBuilder.CreateTable(
                name: "OrdenProcesos",
                columns: table => new
                {
                    OrdenProcesoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrdenProduccionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcesoFabricacionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Completado = table.Column<bool>(type: "INTEGER", nullable: false),
                    FechaCompletado = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Secuencia = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenProcesos", x => x.OrdenProcesoId);
                    table.ForeignKey(
                        name: "FK_OrdenProcesos_OrdenesProduccion_OrdenProduccionId",
                        column: x => x.OrdenProduccionId,
                        principalTable: "OrdenesProduccion",
                        principalColumn: "OrdenProduccionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenProcesos_ProcesosFabricacion_ProcesoFabricacionId",
                        column: x => x.ProcesoFabricacionId,
                        principalTable: "ProcesosFabricacion",
                        principalColumn: "ProcesoFabricacionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ProcesosFabricacion",
                columns: new[] { "ProcesoFabricacionId", "Descripcion", "DuracionEstimadaHoras", "Nombre" },
                values: new object[,]
                {
                    { 1, "Corte de piezas de cuero y materiales según molde.", 4, "Corte" },
                    { 2, "Unión de piezas cortadas mediante costura.", 6, "Costura" },
                    { 3, "Ensamblaje de la suela con el corte cosido.", 5, "Ensamblado" },
                    { 4, "Inspección final de defectos y acabados.", 2, "Control de Calidad" },
                    { 5, "Empaque final del producto para distribución.", 1, "Empaque" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesProduccion_Codigo",
                table: "OrdenesProduccion",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdenProcesos_OrdenProduccionId_ProcesoFabricacionId",
                table: "OrdenProcesos",
                columns: new[] { "OrdenProduccionId", "ProcesoFabricacionId" },
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
