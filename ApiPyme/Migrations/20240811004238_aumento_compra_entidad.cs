using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPyme.Migrations
{
    /// <inheritdoc />
    public partial class aumento_compra_entidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    id_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero_compra = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_comprobante = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_compra = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    total_compra = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    iva = table.Column<int>(type: "int", nullable: false),
                    id_usuario_proveedor = table.Column<int>(type: "int", nullable: true),
                    id_usuario_comerciante = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    observacion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_modificacion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.id_compra);
                    table.ForeignKey(
                        name: "FK_Compra_Usuario_id_usuario_comerciante",
                        column: x => x.id_usuario_comerciante,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "FK_Compra_Usuario_id_usuario_proveedor",
                        column: x => x.id_usuario_proveedor,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DetalleCompra",
                columns: table => new
                {
                    id_detalle_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cantidad_inicial = table.Column<int>(type: "int", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    productoIdProducto = table.Column<int>(type: "int", nullable: true),
                    id_compra = table.Column<int>(type: "int", nullable: true),
                    compraIdCompra = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    observacion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_modificacion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleCompra", x => x.id_detalle_compra);
                    table.ForeignKey(
                        name: "FK_DetalleCompra_Compra_compraIdCompra",
                        column: x => x.compraIdCompra,
                        principalTable: "Compra",
                        principalColumn: "id_compra");
                    table.ForeignKey(
                        name: "FK_DetalleCompra_Producto_productoIdProducto",
                        column: x => x.productoIdProducto,
                        principalTable: "Producto",
                        principalColumn: "id_producto");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_id_usuario_comerciante",
                table: "Compra",
                column: "id_usuario_comerciante");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_id_usuario_proveedor",
                table: "Compra",
                column: "id_usuario_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_compraIdCompra",
                table: "DetalleCompra",
                column: "compraIdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_productoIdProducto",
                table: "DetalleCompra",
                column: "productoIdProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleCompra");

            migrationBuilder.DropTable(
                name: "Compra");
        }
    }
}
