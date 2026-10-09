using Microsoft.AspNetCore.Mvc;
using TiendaRopa.Repositorio.Repositorios.VentasRep;
using TiendaRopa.Shared.DTO.Cliente;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using TiendaRopa.Shared.DTO.Venta;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Server.Controllers
{
    // [Authorize(Roles = "Administrador")]  // mismo criterio que ProductoController: se habilita al activar los roles
    [ApiController]
    [Route("api/Venta")]
    public class VentaController : ControllerBase
    {
        private readonly IVentaRepositorio repositorio;

        public VentaController(IVentaRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        // ------------------------------------------------------------------
        // Historial y detalle
        // ------------------------------------------------------------------

        // api/Venta/historial?canal=Presencial&desde=...&hasta=...&clienteId=...
        [HttpGet("historial")]
        public async Task<ActionResult<List<VentaMostrarDTO>>> GetHistorial(
            [FromQuery] CanalVenta? canal, [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta, [FromQuery] int? clienteId)
        {
            // A diferencia de las listas de producto, una lista vacía es una respuesta válida (todavía no hay ventas).
            var lista = await repositorio.GetHistorial(canal, desde, hasta, clienteId);
            return Ok(lista);
        }

        [HttpGet("{id:int}")] //api/Venta/{id}
        public async Task<ActionResult<VentaDetalleDTO>> GetById(int id)
        {
            var entidad = await repositorio.ObtenerById(id);
            if (entidad == null)
            {
                return NotFound($"No existe el registro con ID: {id}.");
            }
            return Ok(entidad);
        }

        // ------------------------------------------------------------------
        // Alta, edición y anulación (ventas presenciales del administrador)
        // ------------------------------------------------------------------

        [HttpPost("crear")] //api/Venta/crear
        public async Task<ActionResult<int>> Post(VentaCrearDTO dto)
        {
            // El canal lo fija el servidor: este endpoint es solo para ventas en el local.
            // La compra web va a llamar a repositorio.Crear(dto, CanalVenta.Web, carritoId) desde su propio controller.
            var resultado = await repositorio.Crear(dto, CanalVenta.Presencial);
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }
            return CreatedAtAction(nameof(GetById), new { id = resultado.VentaId }, resultado.VentaId);
        }

        [HttpPut("editar/{id:int}")] //api/Venta/editar/{id}
        public async Task<ActionResult> Put(int id, VentaCrearDTO dto)
        {
            var resultado = await repositorio.Editar(id, dto);
            if (resultado.NoEncontrada)
            {
                return NotFound(resultado.Error);
            }
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }
            return Ok($"Venta {id} actualizada correctamente.");
        }

        [HttpDelete("anular/{id:int}")] //api/Venta/anular/{id}
        public async Task<ActionResult> Anular(int id)
        {
            var resultado = await repositorio.Anular(id);
            if (resultado.NoEncontrada)
            {
                return NotFound(resultado.Error);
            }
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }
            return Ok($"Venta {id} anulada correctamente. El stock fue repuesto.");
        }

        // ------------------------------------------------------------------
        // Datos de apoyo para la pantalla de venta
        // ------------------------------------------------------------------

        [HttpGet("clientes")] //api/Venta/clientes
        public async Task<ActionResult<List<ClienteMostrarDTO>>> GetClientes()
        {
            return Ok(await repositorio.GetClientesActivos());
        }

        [HttpGet("variantes-disponibles")] //api/Venta/variantes-disponibles
        public async Task<ActionResult<List<VarianteMostrarDTO>>> GetVariantes()
        {
            return Ok(await repositorio.GetVariantesDisponibles());
        }

        [HttpGet("metodos-pago")] //api/Venta/metodos-pago
        public async Task<ActionResult<List<PagoMostrarDTO>>> GetMetodosPago()
        {
            return Ok(await repositorio.GetMetodosPago());
        }
    }
}
