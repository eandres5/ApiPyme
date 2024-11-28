using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPyme.Migrations
{
    /// <inheritdoc />
    public partial class aumento_compra_detalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompra_Compra_compraIdCompra",
                table: "DetalleCompra");

            migrationBuilder.DropIndex(
                name: "IX_DetalleCompra_compraIdCompra",
                table: "DetalleCompra");

            migrationBuilder.DropColumn(
                name: "compraIdCompra",
                table: "DetalleCompra");

            migrationBuilder.AddColumn<string>(
                name: "estado_producto",
                table: "Producto",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "precio_unitario",
                table: "DetalleCompra",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_id_compra",
                table: "DetalleCompra",
                column: "id_compra");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompra_Compra_id_compra",
                table: "DetalleCompra",
                column: "id_compra",
                principalTable: "Compra",
                principalColumn: "id_compra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompra_Compra_id_compra",
                table: "DetalleCompra");

            migrationBuilder.DropIndex(
                name: "IX_DetalleCompra_id_compra",
                table: "DetalleCompra");

            migrationBuilder.DropColumn(
                name: "estado_producto",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "precio_unitario",
                table: "DetalleCompra");

            migrationBuilder.AddColumn<int>(
                name: "compraIdCompra",
                table: "DetalleCompra",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_compraIdCompra",
                table: "DetalleCompra",
                column: "compraIdCompra");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompra_Compra_compraIdCompra",
                table: "DetalleCompra",
                column: "compraIdCompra",
                principalTable: "Compra",
                principalColumn: "id_compra");
        }
    }
}
