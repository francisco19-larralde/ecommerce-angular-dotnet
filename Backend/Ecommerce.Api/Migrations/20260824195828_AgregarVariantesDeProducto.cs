using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarVariantesDeProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TieneVariantes",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VarianteId",
                table: "CarritoItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductoVariantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Talle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoVariantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoVariantes_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarritoItems_VarianteId",
                table: "CarritoItems",
                column: "VarianteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVariantes_ProductoId",
                table: "ProductoVariantes",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoItems_ProductoVariantes_VarianteId",
                table: "CarritoItems",
                column: "VarianteId",
                principalTable: "ProductoVariantes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoItems_ProductoVariantes_VarianteId",
                table: "CarritoItems");

            migrationBuilder.DropTable(
                name: "ProductoVariantes");

            migrationBuilder.DropIndex(
                name: "IX_CarritoItems_VarianteId",
                table: "CarritoItems");

            migrationBuilder.DropColumn(
                name: "TieneVariantes",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "VarianteId",
                table: "CarritoItems");
        }
    }
}
