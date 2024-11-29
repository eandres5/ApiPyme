using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPyme.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    table.PrimaryKey("PK_Categoria", x => x.id_categoria);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    table.PrimaryKey("PK_Rol", x => x.id_rol);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombres = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellidos = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    identificacion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    table.PrimaryKey("PK_Usuario", x => x.id_usuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "Comprobante",
                columns: table => new
                {
                    id_comprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero_comprobante = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_comprobante = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_transaccion = table.Column<string>(type: "longtext", nullable: false)
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
                name: "Pedido",
                columns: table => new
                {
                    id_pedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero_pedido = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    table.PrimaryKey("PK_Pedido", x => x.id_pedido);
                    table.ForeignKey(
                        name: "FK_Pedido_Usuario_id_usuario_comerciante",
                        column: x => x.id_usuario_comerciante,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "FK_Pedido_Usuario_id_usuario_proveedor",
                        column: x => x.id_usuario_proveedor,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre_producto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stock = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    nombre_categoria = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_usuario_proveedor = table.Column<int>(type: "int", nullable: true),
                    qr_path = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado_producto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaIdCategoria = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Producto", x => x.id_producto);
                    table.ForeignKey(
                        name: "FK_Producto_Categoria_CategoriaIdCategoria",
                        column: x => x.CategoriaIdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "id_categoria");
                    table.ForeignKey(
                        name: "FK_Producto_Usuario_id_usuario_proveedor",
                        column: x => x.id_usuario_proveedor,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                columns: table => new
                {
                    id_usuario_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    id_rol = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_UsuarioRol", x => x.id_usuario_rol);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Rol_id_rol",
                        column: x => x.id_rol,
                        principalTable: "Rol",
                        principalColumn: "id_rol");
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuario_id_usuario",
                        column: x => x.id_usuario,
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
                    precio_unitario = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
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
                        name: "FK_DetalleCompra_Compra_id_compra",
                        column: x => x.id_compra,
                        principalTable: "Compra",
                        principalColumn: "id_compra");
                    table.ForeignKey(
                        name: "FK_DetalleCompra_Producto_productoIdProducto",
                        column: x => x.productoIdProducto,
                        principalTable: "Producto",
                        principalColumn: "id_producto");
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

            migrationBuilder.CreateTable(
                name: "DetallePedido",
                columns: table => new
                {
                    id_detalle_pedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    estado_pedido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    id_pedido = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_DetallePedido", x => x.id_detalle_pedido);
                    table.ForeignKey(
                        name: "FK_DetallePedido_Pedido_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "Pedido",
                        principalColumn: "id_pedido");
                    table.ForeignKey(
                        name: "FK_DetallePedido_Producto_id_producto",
                        column: x => x.id_producto,
                        principalTable: "Producto",
                        principalColumn: "id_producto");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    id_inventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_comerciante = table.Column<int>(type: "int", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    usuarioComercianteIdUsuario = table.Column<int>(type: "int", nullable: true),
                    productoIdProducto = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Inventario", x => x.id_inventario);
                    table.ForeignKey(
                        name: "FK_Inventario_Producto_productoIdProducto",
                        column: x => x.productoIdProducto,
                        principalTable: "Producto",
                        principalColumn: "id_producto");
                    table.ForeignKey(
                        name: "FK_Inventario_Usuario_usuarioComercianteIdUsuario",
                        column: x => x.usuarioComercianteIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
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
                name: "IX_Comprobante_id_usuario_cliente",
                table: "Comprobante",
                column: "id_usuario_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobante_id_usuario_comerciante",
                table: "Comprobante",
                column: "id_usuario_comerciante");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_id_compra",
                table: "DetalleCompra",
                column: "id_compra");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_productoIdProducto",
                table: "DetalleCompra",
                column: "productoIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleComprobante_id_comprobante",
                table: "DetalleComprobante",
                column: "id_comprobante");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleComprobante_productoIdProducto",
                table: "DetalleComprobante",
                column: "productoIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedido_id_pedido",
                table: "DetallePedido",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedido_id_producto",
                table: "DetallePedido",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_productoIdProducto",
                table: "Inventario",
                column: "productoIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_usuarioComercianteIdUsuario",
                table: "Inventario",
                column: "usuarioComercianteIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_id_usuario_comerciante",
                table: "Pedido",
                column: "id_usuario_comerciante");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_id_usuario_proveedor",
                table: "Pedido",
                column: "id_usuario_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CategoriaIdCategoria",
                table: "Producto",
                column: "CategoriaIdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_id_usuario_proveedor",
                table: "Producto",
                column: "id_usuario_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_id_rol",
                table: "UsuarioRol",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_id_usuario",
                table: "UsuarioRol",
                column: "id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleCompra");

            migrationBuilder.DropTable(
                name: "DetalleComprobante");

            migrationBuilder.DropTable(
                name: "DetallePedido");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "UsuarioRol");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "Comprobante");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
