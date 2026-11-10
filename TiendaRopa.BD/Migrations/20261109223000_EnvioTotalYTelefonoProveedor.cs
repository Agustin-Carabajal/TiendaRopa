using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TiendaRopa.BD.Datos;

#nullable disable

namespace TiendaRopa.BD.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261109223000_EnvioTotalYTelefonoProveedor")]
    public partial class EnvioTotalYTelefonoProveedor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecioEnvioTotal",
                table: "Recepciones",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoContacto",
                table: "Proveedores",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "PrecioEnvioTotal", table: "Recepciones");
            migrationBuilder.DropColumn(name: "TelefonoContacto", table: "Proveedores");
        }
    }
}
