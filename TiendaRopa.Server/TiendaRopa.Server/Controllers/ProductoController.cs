using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.ProductoRep;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Server.Controllers
{
    [ApiController]
    [Route("api/Producto")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepositorio repositorio;
        private readonly AppDbContext context;

        public ProductoController(AppDbContext context, IProductoRepositorio repositorio)
        {
            this.context = context;
            this.repositorio = repositorio;
        }
        [HttpGet("lista-productos-activos")] //api/Producto/lista-productos-activos
        public async Task<ActionResult<List<ProductoMostrarDTO>>> GetLista()
        {
            var lista = await repositorio.GetListaActivos();
            if (lista == null)
            {
                return NotFound("No se encontraron elementos de la lista, VERIFICAR.");
            }
            if (lista.Count == 0)
            {
                return NotFound("Lista sin registros.");
            }
            return Ok(lista);
        }

        [HttpGet("lista-productos-inactivos")] //api/Producto/lista-productos-inactivos
        public async Task<ActionResult<List<ProductoMostrarDTO>>> GetListaInactivos()
        {
            var lista = await repositorio.GetListaInactivos();
            if (lista == null)
            {
                return NotFound("No se encontraron elementos de la lista, VERIFICAR.");
            }
            if (lista.Count == 0)
            {
                return NotFound("Lista sin registros.");
            }
            return Ok(lista);
        }

        [HttpGet("{id:int}")] //api/Producto/{id}
        public async Task<ActionResult<ProductoMostrarDTO>> GetById(int id)
        {
            var entidad = await repositorio.ObtenerById(id);
            if (entidad == null)
            {
                return NotFound($"No se existe el registro con ID: {id}.");
            }
            return Ok(entidad);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("crear")] //api/Producto/crear
        public async Task<ActionResult<int>> Post(ProductoCrearDTO DTO)
        {
           
            Producto entidad = new Producto
            {
                NombreProducto = DTO.Nombre,
                DescripcionProducto = DTO.Descripcion,
                MarcaProducto = DTO.Marca,
                ProveedorId = DTO.ProveedorId,
                EstadoRegistro = DTO.Estado
            };

            var id = await repositorio.Insert(entidad);

            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad.Id); ;
        }

        [HttpPost("registrar-completo")]
        public async Task<ActionResult> RegistrarProductoCompleto([FromBody] RegistrarProductoCompletoDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Iniciamos una transacción usando el contexto de EF Core
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // 1. Crear e insertar el Producto básico
                var nuevoProducto = new Producto
                {
                    NombreProducto = dto.NombreProducto,
                    DescripcionProducto = dto.DescripcionProducto,
                    MarcaProducto = dto.MarcaProducto,
                    ProveedorId = dto.ProveedorId,
                    EstadoRegistro = dto.Activo ? EstadoRegistro.activo : EstadoRegistro.inactivo

                    // Si maneja 'Activo' u otros campos base, los asignas aquí
                };

                context.Productos.Add(nuevoProducto);
                await context.SaveChangesAsync(); // Genera el nuevoProducto.Id

                // 2. Recorrer los colores seleccionados
                foreach (var colorDto in dto.Colores)
                {
                    var nuevoProductoColor = new ProductoColor
                    {
                        ProductoId = nuevoProducto.Id,
                        ColorId = colorDto.ColorId,
                        UrlImagen = colorDto.UrlImagen,
                        EstadoRegistro = EstadoRegistro.activo // Asegúrate de heredar el estado activo
                    };

                    context.ProductosColores.Add(nuevoProductoColor);
                    await context.SaveChangesAsync(); // 🌟 ESTA LÍNEA ES CRUCIAL para que se genere el nuevoProductoColor.Id

                    foreach (var varianteDto in colorDto.Variantes)
                    {
                        var nuevaVariante = new Variante
                        {
                            // 🛑 REVISA ESTA LÍNEA: Debe apuntar al ID generado arriba
                            ProductoColorId = nuevoProductoColor.Id,
                            TalleId = varianteDto.TalleId,
                            Stock = varianteDto.Stock,
                            PrecioVenta = varianteDto.PrecioVenta,
                            CodVariante = varianteDto.CodVariante,
                            EstadoRegistro = EstadoRegistro.activo // 🌟 OBLIGATORIO: Ponlo en activo para que el GET lo lea
                        };

                        context.Variantes.Add(nuevaVariante);
                    }
                }
           

                // 4. Guardar todas las variantes juntas
                await context.SaveChangesAsync();

                // 5. Confirmar la transacción en la base de datos
                await transaction.CommitAsync();

                return Ok(new { Mensaje = "Producto con todas sus variantes registrado con éxito.", ProductoId = nuevoProducto.Id });
            }
            catch (Exception ex)
            {
                // Si algo falla, se deshacen todos los inserts automáticamente
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error interno al registrar el producto: {ex.Message}");
            }
        }

        // [Authorize(Roles = "Administrador")]
        [HttpPut("editar/{id:int}")] //api/Producto/editar/{id}
        public async Task<ActionResult> Put(int id, ProductoCrearDTO DTO)
        {
            var flag = await repositorio.Editar(id, DTO);
            if (!flag)
            {
                return BadRequest("Datos no validos o el registro no existe.");
            }
            return Ok($"Registro con el id: {id} actualizado correctamente.");
        }

        // [Authorize(Roles = "Administrador")]
        [HttpDelete("borrar/{id:int}")] //api/Producto/Borrar/{id}
        public async Task<ActionResult> Delete(int id)
        {
            var flag = await repositorio.DeleteLogico(id);
            if (!flag)
            {
                return NotFound($"No existe el registro con el id: {id} o ya fue eliminado.");
            }
            return Ok($"Registro con el id: {id} eliminado correctamente.");
        }
    }
}
