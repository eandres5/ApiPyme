using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPyme.Migrations
{
    /// <inheritdoc />
    public partial class entidad_comprobante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comprobante",
                columns: table => new
                {
                    id_comprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero_comprobante = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_comprobante = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_emision = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    iva = table.Column<int>(type: "int", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    id_usuario_cliente = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Comprobante", x => x.id_comprobante);
                    table.ForeignKey(
                        name: "FK_Comprobante_Usuario_id_usuario_cliente",
                        column: x => x.id_usuario_cliente,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "FK_Comprobante_Usuario_id_usuario_comerciante",
                        column: x => x.id_usuario_comerciante,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DetalleComprobante",
                columns: table => new
                {
                    id_detalle_comprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    precio_unitario = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    productoIdProducto = table.Column<int>(type: "int", nullable: true),
                    id_comprobante = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_DetalleComprobante", x => x.id_detalle_comprobante);
                    table.ForeignKey(
                        name: "FK_DetalleComprobante_Comprobante_id_comprobante",
                        column: x => x.id_comprobante,
                        principalTable: "Comprobante",
                        principalColumn: "id_comprobante");
                    table.ForeignKey(
                        name: "FK_DetalleComprobante_Producto_productoIdProducto",
                        column: x => x.productoIdProducto,
                        principalTable: "Producto",
                        principalColumn: "id_producto");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobante_id_usuario_cliente",
                table: "Comprobante",
                column: "id_usuario_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobante_id_usuario_comerciante",
                table: "Comprobante",
                column: "id_usuario_comerciante");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleComprobante_id_comprobante",
                table: "DetalleComprobante",
                column: "id_comprobante");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleComprobante_productoIdProducto",
                table: "DetalleComprobante",
                column: "productoIdProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleComprobante");

            migrationBuilder.DropTable(
                name: "Comprobante");
        }
    }
}
