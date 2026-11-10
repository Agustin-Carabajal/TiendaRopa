using System.Globalization;
using System.Text;
using TiendaRopa.BD.Datos.Entity;

namespace TiendaRopa.Server.Services;

public static class PedidoPdfGenerator
{
    private const int AnchoPagina = 595;

    public static byte[] Generar(Pedido pedido, string proveedor, IReadOnlyList<DetallesPedido> detalles)
    {
        var paginas = new List<StringBuilder>();
        var pagina = NuevaPagina(paginas, pedido, proveedor);
        var y = 648;

        foreach (var detalle in detalles)
        {
            if (y < 90)
            {
                pagina = NuevaPagina(paginas, pedido, proveedor);
                y = 648;
            }

            var producto = detalle.Variante?.ProductoColor?.Producto?.NombreProducto ?? "Producto";
            var color = detalle.Variante?.ProductoColor?.Color?.NombreColor ?? "Color sin asignar";
            var talle = detalle.Variante?.Talle?.NombreTalle ?? "Sin talle";
            var nombreVariante = LimitarTexto($"{producto} - {color} - Talle {talle}", 65);
            var sku = detalle.Variante?.CodVariante ?? "-";

            Texto(pagina, 42, y, 9, nombreVariante);
            Texto(pagina, 440, y, 9, LimitarTexto(sku, 15));
            TextoDerecha(pagina, 553, y, 9, detalle.Cant_prod_Pedido.ToString(CultureInfo.InvariantCulture));
            Linea(pagina, 42, y - 7, 553, y - 7);
            y -= 22;
        }

        if (y < 90)
        {
            pagina = NuevaPagina(paginas, pedido, proveedor);
            y = 648;
        }

        Texto(pagina, 42, y - 10, 9, "El costo unitario, el envío y la ganancia se determinan al registrar la recepción.");

        return CrearPdf(paginas);
    }

    private static StringBuilder NuevaPagina(List<StringBuilder> paginas, Pedido pedido, string proveedor)
    {
        var pagina = new StringBuilder();
        paginas.Add(pagina);

        Texto(pagina, 42, 800, 18, "RECIBO DE PEDIDO A PROVEEDOR");
        Texto(pagina, 42, 776, 10, "Comprobante de los artículos solicitados");
        Texto(pagina, 42, 742, 10, $"Factura / pedido: {pedido.FacturaPedidos}   |   Pedido N.º {pedido.Id}");
        Texto(pagina, 42, 723, 10, $"Proveedor: {proveedor}");
        Texto(pagina, 42, 704, 10, $"Emitido: {pedido.FechaDePedido:dd/MM/yyyy}   |   Entrega estimada: {(pedido.FechaDeEntrega.HasValue ? pedido.FechaDeEntrega.Value.ToString("dd/MM/yyyy") : "Pendiente")}");
        Linea(pagina, 42, 690, AnchoPagina - 42, 690);

        Texto(pagina, 42, 674, 8, "VARIANTE");
        Texto(pagina, 440, 674, 8, "SKU");
        TextoDerecha(pagina, 553, 674, 8, "CANTIDAD");
        Linea(pagina, 42, 668, AnchoPagina - 42, 668);
        return pagina;
    }

    private static byte[] CrearPdf(IReadOnlyList<StringBuilder> paginas)
    {
        var objetos = new List<byte[]>
        {
            Encoding.ASCII.GetBytes("<< /Type /Catalog /Pages 2 0 R >>"),
            Encoding.ASCII.GetBytes($"<< /Type /Pages /Kids [{string.Join(" ", Enumerable.Range(0, paginas.Count).Select(i => $"{4 + i * 2} 0 R"))}] /Count {paginas.Count} >>"),
            Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>")
        };

        for (var indicePagina = 0; indicePagina < paginas.Count; indicePagina++)
        {
            var pagina = paginas[indicePagina];
            var contenido = Encoding.Latin1.GetBytes(pagina.ToString());
            var idPagina = 4 + indicePagina * 2;
            var idContenido = idPagina + 1;
            objetos.Add(Encoding.ASCII.GetBytes($"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {AnchoPagina} 842] /Resources << /Font << /F1 3 0 R >> >> /Contents {idContenido} 0 R >>"));

            using var flujo = new MemoryStream();
            Escribir(flujo, Encoding.ASCII.GetBytes($"<< /Length {contenido.Length} >>\nstream\n"));
            Escribir(flujo, contenido);
            Escribir(flujo, Encoding.ASCII.GetBytes("\nendstream"));
            objetos.Add(flujo.ToArray());
        }

        using var pdf = new MemoryStream();
        Escribir(pdf, Encoding.ASCII.GetBytes("%PDF-1.4\n"));
        var offsets = new List<long> { 0 };
        foreach (var objeto in objetos)
        {
            var objectId = offsets.Count;
            offsets.Add(pdf.Position);
            Escribir(pdf, Encoding.ASCII.GetBytes($"{objectId} 0 obj\n"));
            Escribir(pdf, objeto);
            Escribir(pdf, Encoding.ASCII.GetBytes("\nendobj\n"));
        }

        var inicioXref = pdf.Position;
        Escribir(pdf, Encoding.ASCII.GetBytes($"xref\n0 {objetos.Count + 1}\n0000000000 65535 f \n"));
        foreach (var offset in offsets.Skip(1))
            Escribir(pdf, Encoding.ASCII.GetBytes($"{offset:0000000000} 00000 n \n"));

        Escribir(pdf, Encoding.ASCII.GetBytes($"trailer\n<< /Size {objetos.Count + 1} /Root 1 0 R >>\nstartxref\n{inicioXref}\n%%EOF"));
        return pdf.ToArray();
    }

    private static void Texto(StringBuilder pagina, int x, int y, int tamano, string texto) =>
        pagina.AppendLine($"BT /F1 {tamano} Tf {x} {y} Td ({Escapar(texto)}) Tj ET");

    private static void TextoDerecha(StringBuilder pagina, int x, int y, int tamano, string texto) =>
        Texto(pagina, x - texto.Length * (tamano / 2), y, tamano, texto);

    private static void Linea(StringBuilder pagina, int x1, int y1, int x2, int y2) =>
        pagina.AppendLine($"0.6 w {x1} {y1} m {x2} {y2} l S");

    private static string Escapar(string texto) => texto
        .Replace("\\", "\\\\")
        .Replace("(", "\\(")
        .Replace(")", "\\)")
        .Replace("\r", " ")
        .Replace("\n", " ");

    private static string LimitarTexto(string texto, int maximo) =>
        texto.Length <= maximo ? texto : texto[..(maximo - 3)] + "...";

    private static void Escribir(Stream destino, byte[] bytes) => destino.Write(bytes, 0, bytes.Length);
}
