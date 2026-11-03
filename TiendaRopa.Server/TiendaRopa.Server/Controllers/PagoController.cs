using Microsoft.AspNetCore.Mvc;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.VentasRep;
using TiendaRopa.Shared.DTO.Venta;
using TiendaRopa.Shared.ENUM;
namespace TiendaRopa.Server.Controllers;
[ApiController,Route("api/Pago")] public class PagoController:ControllerBase { private readonly IPagoRepositorio repo; public PagoController(IPagoRepositorio repo)=>this.repo=repo; [HttpGet] public async Task<ActionResult<List<PagoDTO>>> Get()=>Ok((await repo.ListarActivos()).Select(p=>new PagoDTO{Id=p.Id,Tipo=p.Tipo}).ToList()); [HttpPost] public async Task<ActionResult<int>> Post(PagoDTO dto){if(string.IsNullOrWhiteSpace(dto.Tipo))return BadRequest("El tipo de pago es obligatorio.");var p=new Pago{Tipo=dto.Tipo.Trim(),EstadoRegistro=EstadoRegistro.activo};var id=await repo.Insert(p);return Ok(id);} [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id,PagoDTO dto){var p=await repo.SelectById(id);if(p==null)return NotFound();p.Tipo=dto.Tipo.Trim();return await repo.Update(id,p)?Ok():BadRequest();} [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id)=>await repo.DeleteLogico(id)?Ok():NotFound(); }
