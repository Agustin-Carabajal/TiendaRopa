using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TiendaRopa.BD.Datos;

#nullable disable

namespace TiendaRopa.BD.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261109224500_DatosAdicionalesProveedorOpcionales")]
    public partial class DatosAdicionalesProveedorOpcionales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CuitProveedores", table: "Proveedores",
                type: "character varying(50)", maxLength: 50, nullable: true,
                oldClrType: typeof(string), oldType: "character varying(50)", oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DomicilioProveedores", table: "Proveedores",
                type: "character varying(200)", maxLength: 200, nullable: true,
                oldClrType: typeof(string), oldType: "character varying(200)", oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ContactoNombreProveedores", table: "Proveedores",
                type: "character varying(50)", maxLength: 50, nullable: true,
                oldClrType: typeof(string), oldType: "character varying(50)", oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "EmailProveedores", table: "Proveedores",
                type: "character varying(50)", maxLength: 50, nullable: true,
                oldClrType: typeof(string), oldType: "character varying(50)", oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "Proveedores" SET
                    "CuitProveedores" = COALESCE("CuitProveedores", ''),
                    "DomicilioProveedores" = COALESCE("DomicilioProveedores", ''),
                    "ContactoNombreProveedores" = COALESCE("ContactoNombreProveedores", ''),
                    "EmailProveedores" = COALESCE("EmailProveedores", '');
                """);

            migrationBuilder.AlterColumn<string>(
                name: "CuitProveedores", table: "Proveedores",
                type: "character varying(50)", maxLength: 50, nullable: false,
                oldClrType: typeof(string), oldType: "character varying(50)", oldMaxLength: 50, oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DomicilioProveedores", table: "Proveedores",
                type: "character varying(200)", maxLength: 200, nullable: false,
                oldClrType: typeof(string), oldType: "character varying(200)", oldMaxLength: 200, oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactoNombreProveedores", table: "Proveedores",
                type: "character varying(50)", maxLength: 50, nullable: false,
                oldClrType: typeof(string), oldType: "character varying(50)", oldMaxLength: 50, oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailProveedores", table: "Proveedores",
                type: "character varying(50)", maxLength: 50, nullable: false,
                oldClrType: typeof(string), oldType: "character varying(50)", oldMaxLength: 50, oldNullable: true);
        }
    }
}
