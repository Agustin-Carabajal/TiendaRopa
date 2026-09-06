namespace TiendaRopa.Shared.ENUM;

public enum OrigenComprador
{
    Presencial,
    Web
}

public enum CanalVenta
{
    Presencial,
    Web
}

public enum TipoMovimientoCuentaCorriente
{
    Cargo,   // aumenta la deuda del cliente (venta fiada)
    Pago     // reduce la deuda del cliente
}

public enum TipoNotificacion
{
    CumpleañosCliente,
    EntregaProxima,
    Otro
}

public enum TipoEntidadNotificacion
{
    Cliente,
    ApplicationUser,
    Pedido
}
