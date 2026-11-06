using Microsoft.AspNetCore.Mvc;
using TiendaRopa.Repositorio.Repositorios.VentasRep;
using TiendaRopa.Shared.DTO.Venta;
namespace TiendaRopa.Server.Controllers;
[ApiController,Route("api/CuentaCorriente")] public class CuentaCorrienteController:ControllerBase { private readonly ICuentaCorrienteRepositorio repo; public CuentaCorrienteController(ICuentaCorrienteRepositorio repo)=>this.repo=repo; [HttpGet("{clienteId:int}")] public async Task<ActionResult<List<CuentaCorrienteMovimientoDTO>>> Get(int clienteId)=>Ok(await repo.Historial(clienteId)); [HttpPost("{clienteId:int}/pagos")] public async Task<IActionResult> Post(int clienteId,[FromBody] CuentaCorrientePagoDTO dto){if(dto.Monto<=0)return BadRequest("El importe debe ser mayor a cero.");return await repo.RegistrarPago(clienteId,dto.Monto)?Ok():NotFound("No existe el cliente.");} }
