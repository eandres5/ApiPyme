using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPyme.Migrations
{
    /// <inheritdoc />
    public partial class entidad_comprobante_tipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tipo_transaccion",
                table: "Comprobante",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tipo_transaccion",
                table: "Comprobante");
        }
    }
}
