using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPyme.Migrations
{
    /// <inheritdoc />
    public partial class mailusuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mail",
                table: "Usuario",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mail",
                table: "Usuario");
        }
    }
}
