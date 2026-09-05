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
                        EstadoRegistro = EstadoRegistro.activo 
                    };

                    context.ProductosColores.Add(nuevoProductoColor);
                    await context.SaveChangesAsync();

                    foreach (var varianteDto in colorDto.Variantes)
                    {
                        var nuevaVariante = new Variante
                        {
                            
                            ProductoColorId = nuevoProductoColor.Id,
                            TalleId = varianteDto.TalleId,
                            Stock = varianteDto.Stock,
                            PrecioVenta = varianteDto.PrecioVenta,
                            CodVariante = varianteDto.CodVariante,
                            EstadoRegistro = EstadoRegistro.activo 
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
        [HttpPut("editar-completo/{id:int}")] //api/Producto/editar-completo/{id}
        public async Task<ActionResult> EditarProductoCompleto(int id, [FromBody] RegistrarProductoCompletoDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var producto = await context.Productos.FirstOrDefaultAsync(p => p.Id == id);
                if (producto == null) return NotFound($"No existe el producto con id {id}.");

                // 1. Actualizar datos básicos del producto
                producto.NombreProducto = dto.NombreProducto;
                producto.DescripcionProducto = dto.DescripcionProducto;
                producto.MarcaProducto = dto.MarcaProducto;
                producto.ProveedorId = dto.ProveedorId;
                producto.EstadoRegistro = dto.Activo ? EstadoRegistro.activo : EstadoRegistro.inactivo;

                // 2. Traer los ProductoColor y Variantes que YA existen para este producto
                var coloresExistentes = await context.ProductosColores
                    .Where(pc => pc.ProductoId == id)
                    .ToListAsync();

                var idsColoresExistentes = coloresExistentes.Select(pc => pc.Id).ToList();
                var variantesExistentes = await context.Variantes
                    .Where(v => idsColoresExistentes.Contains(v.ProductoColorId))
                    .ToListAsync();

                // 3. Recorrer los colores que llegan del formulario de edición
                foreach (var colorDto in dto.Colores)
                {
                    var productoColor = coloresExistentes.FirstOrDefault(pc => pc.ColorId == colorDto.ColorId);

                    if (productoColor == null)
                    {
                        // Color nuevo para este producto: se crea el ProductoColor y sus variantes
                        productoColor = new ProductoColor
                        {
                            ProductoId = id,
                            ColorId = colorDto.ColorId,
                            UrlImagen = colorDto.UrlImagen,
                            EstadoRegistro = EstadoRegistro.activo
                        };
                        context.ProductosColores.Add(productoColor);
                        await context.SaveChangesAsync(); // genera productoColor.Id

                        foreach (var varianteDto in colorDto.Variantes)
                        {
                            context.Variantes.Add(new Variante
                            {
                                ProductoColorId = productoColor.Id,
                                TalleId = varianteDto.TalleId,
                                Stock = varianteDto.Stock,
                                PrecioVenta = varianteDto.PrecioVenta,
                                CodVariante = varianteDto.CodVariante,
                                EstadoRegistro = EstadoRegistro.activo
                            });
                        }
                    }
                    else
                    {
                        // Color existente: se reactiva por si estaba dado de baja, y se actualiza la imagen
                        productoColor.UrlImagen = colorDto.UrlImagen;
                        productoColor.EstadoRegistro = EstadoRegistro.activo;

                        var variantesDeEsteColor = variantesExistentes.Where(v => v.ProductoColorId == productoColor.Id).ToList();

                        foreach (var varianteDto in colorDto.Variantes)
                        {
                            var varianteExistente = variantesDeEsteColor.FirstOrDefault(v => v.TalleId == varianteDto.TalleId);

                            if (varianteExistente == null)
                            {
                                // Talle nuevo dentro de un color que ya existía
                                context.Variantes.Add(new Variante
                                {
                                    ProductoColorId = productoColor.Id,
                                    TalleId = varianteDto.TalleId,
                                    Stock = varianteDto.Stock,
                                    PrecioVenta = varianteDto.PrecioVenta,
                                    CodVariante = varianteDto.CodVariante,
                                    EstadoRegistro = EstadoRegistro.activo
                                });
                            }
                            else
                            {
                                // Talle existente: se actualizan sus datos y se reactiva por si estaba inactivo
                                varianteExistente.Stock = varianteDto.Stock;
                                varianteExistente.PrecioVenta = varianteDto.PrecioVenta;
                                varianteExistente.CodVariante = varianteDto.CodVariante;
                                varianteExistente.EstadoRegistro = EstadoRegistro.activo;
                            }
                        }

                        // Talles que existían para este color pero ya no vienen tildados en el formulario -> baja lógica
                        var talleIdsEnviados = colorDto.Variantes.Select(v => v.TalleId).ToHashSet();
                        foreach (var varianteVieja in variantesDeEsteColor.Where(v => !talleIdsEnviados.Contains(v.TalleId)))
                        {
                            varianteVieja.EstadoRegistro = EstadoRegistro.inactivo;
                        }
                    }
                }

                // 4. Colores que existían antes pero ya no vienen en el formulario -> baja lógica del color y sus variantes
                var colorIdsEnviados = dto.Colores.Select(c => c.ColorId).ToHashSet();
                foreach (var colorViejo in coloresExistentes.Where(pc => !colorIdsEnviados.Contains(pc.ColorId)))
                {
                    colorViejo.EstadoRegistro = EstadoRegistro.inactivo;
                    foreach (var v in variantesExistentes.Where(v => v.ProductoColorId == colorViejo.Id))
                    {
                        v.EstadoRegistro = EstadoRegistro.inactivo;
                    }
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { Mensaje = "Producto actualizado con éxito.", ProductoId = id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error interno al editar el producto: {ex.Message}");
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
