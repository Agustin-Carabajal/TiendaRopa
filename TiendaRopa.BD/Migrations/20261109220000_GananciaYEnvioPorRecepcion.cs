using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TiendaRopa.BD.Datos;

#nullable disable

namespace TiendaRopa.BD.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261109220000_GananciaYEnvioPorRecepcion")]
    public partial class GananciaYEnvioPorRecepcion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ganancia",
                table: "DetallesRecepciones",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioEnvio",
                table: "DetallesRecepciones",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Ganancia", table: "DetallesRecepciones");
            migrationBuilder.DropColumn(name: "PrecioEnvio", table: "DetallesRecepciones");
        }
    }
}
