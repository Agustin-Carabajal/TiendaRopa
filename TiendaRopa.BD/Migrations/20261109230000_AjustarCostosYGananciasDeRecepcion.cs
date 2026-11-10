using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TiendaRopa.BD.Datos;

#nullable disable

namespace TiendaRopa.BD.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261109230000_AjustarCostosYGananciasDeRecepcion")]
    public partial class AjustarCostosYGananciasDeRecepcion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "DetallesRecepciones"
                SET "Ganancia" = "PrecioUnitario" * 2;

                UPDATE "Variantes" AS v
                SET "PrecioVenta" = d."PrecioUnitario" + d."PrecioEnvio" + d."Ganancia"
                FROM (
                    SELECT DISTINCT ON ("VarianteId")
                        "VarianteId", "PrecioUnitario", "PrecioEnvio", "Ganancia"
                    FROM "DetallesRecepciones"
                    ORDER BY "VarianteId", "Id" DESC
                ) AS d
                WHERE v."Id" = d."VarianteId";

                UPDATE "Pedidos" AS p
                SET "TotalPedidos" = COALESCE((
                    SELECT SUM(d."PrecioUnitario" * d."CantidadRecibida")
                    FROM "DetallesRecepciones" AS d
                    WHERE d."PedidoId" = p."Id"
                ), 0);
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Los valores anteriores de ganancia y precio de venta no se pueden
            // reconstruir luego de aplicar la nueva fórmula.
        }
    }
}
