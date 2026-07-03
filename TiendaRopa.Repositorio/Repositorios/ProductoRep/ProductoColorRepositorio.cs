using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopa.BD.Datos;
using TiendaRopa.BD.Datos.Entity;
using TiendaRopa.Repositorio.Repositorios.Generico;
using TiendaRopa.Shared.DTO.Producto_y_mas;
using TiendaRopa.Shared.ENUM;

namespace TiendaRopa.Repositorio.Repositorios.ProductoRep
{
    public class ProductoColorRepositorio : Repositorio<ProductoColor>, IRepositorio<ProductoColor>, IProductoColorRepositorio
    {
        private readonly AppDbContext context;

        public ProductoColorRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<ProductoColorMostrarDTO>> GetListaActivos()
        {
            return await context.ProductosColores.Where(pc => pc.EstadoRegistro == EstadoRegistro.activo)
                .Include(pc => pc.Producto)
                .Include(pc => pc.Color)
                .Select(pc => new ProductoColorMostrarDTO
                {
                    UrlImagen = pc.UrlImagen,
                    // EF Core se encarga de buscar el nombre a través de la relación
                    ProductoNombre = pc.Producto != null ? pc.Producto.NombreProducto : "Sin Producto",
                    ColorNombre = pc.Color != null ? pc.Color.NombreColor : "Sin Color",
                    Estado = pc.EstadoRegistro
                })
                .ToListAsync();
        }

        public async Task<List<ProductoColorMostrarDTO>> GetListaInactivos()
        {
            return await context.ProductosColores.Where(pc => pc.EstadoRegistro == EstadoRegistro.inactivo)
                .Include(pc => pc.Producto)
                .Include(pc => pc.Color)
                .Select(pc => new ProductoColorMostrarDTO
                {
                    UrlImagen = pc.UrlImagen,
                    // EF Core se encarga de buscar el nombre a través de la relación
                    ProductoNombre = pc.Producto != null ? pc.Producto.NombreProducto : "Sin Producto",
                    ColorNombre = pc.Color != null ? pc.Color.NombreColor : "Sin Color",
                    Estado = pc.EstadoRegistro
                })
                .ToListAsync();
        }

        public async Task<bool> Editar(int id, ProductoColorEditarDTO dto)
        {
            // 1. Buscamos el registro real directamente de la base de datos
            var registroExistente = await context.ProductosColores
                .FirstOrDefaultAsync(pc => pc.Id == id);

            if (registroExistente == null) return false;

            // 2. Modificamos únicamente los campos permitidos
            registroExistente.UrlImagen = dto.UrlImagen;
            registroExistente.ProductoId = dto.ProductoId;
            registroExistente.ColorId = dto.ColorId;
            registroExistente.EstadoRegistro = dto.Estado;

            // Si manejas auditoría o fechas de modificación en EntityBase, las actualizas aquí:
            // registroExistente.FechaModificacion = DateTime.Now;

            // 3. Guardamos los cambios (EF Core solo hará el UPDATE de estas columnas)
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductoColorMostrarDTO?> ObtenerById(int id)
        {
            return await context.ProductosColores
                .Where(pc => pc.Id == id)
                .Select(pc => new ProductoColorMostrarDTO
                {
                    UrlImagen = pc.UrlImagen,
                    ProductoNombre = pc.Producto != null ? pc.Producto.NombreProducto : "Sin Producto",
                    ColorNombre = pc.Color != null ? pc.Color.NombreColor : "Sin Color"
                }).FirstOrDefaultAsync();
        }


    }
}
