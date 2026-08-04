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
    public class VarianteRepositorio : Repositorio<Variante>, IRepositorio<Variante>, IVarianteRepositorio
    {
        private readonly AppDbContext context;

        public VarianteRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<VarianteMostrarDTO>> GetListaActivos()
        {
            return await context.Variantes
                .Where(v => v.EstadoRegistro == EstadoRegistro.activo)
                .Include(v => v.ProductoColor)
                    .ThenInclude(pc => pc!.Producto)
                .Include(v => v.ProductoColor)
                    .ThenInclude(pc => pc!.Color)
                .Include(v => v.Talle)
               .Select(v => new VarianteMostrarDTO
               {
                   Id = v.Id,

                   // CORRECCIÓN CLAVE: Navegación profunda en las relaciones para extraer el ID correcto
                   ProductoId = (v.ProductoColor != null && v.ProductoColor.Producto != null)
                 ? v.ProductoColor.Producto.Id
                 : 0,

                   CodVariante = v.CodVariante,
                   Stock = v.Stock,
                   PrecioVenta = v.PrecioVenta,
                   Estado = v.EstadoRegistro,
                   Talle = v.Talle != null ? v.Talle.NombreTalle : "Sin Talle",
                   ProductoColor = v.ProductoColor != null && v.ProductoColor.Producto != null && v.ProductoColor.Color != null
                        ? $"{v.ProductoColor.Producto.NombreProducto} - {v.ProductoColor.Color.NombreColor}"
                        : "Producto/Color no asignado"
               })
               .ToListAsync();
        }

        public async Task<VarianteMostrarDTO?> ObtenerById(int id)
        {
            return await context.Variantes
                .Where(v => v.Id == id)
                .Include(v => v.ProductoColor)
                    .ThenInclude(pc => pc!.Producto)
                .Include(v => v.ProductoColor)
                    .ThenInclude(pc => pc!.Color)
                .Include(v => v.Talle)
                .Select(v => new VarianteMostrarDTO
                {
                    Id = v.Id,
                    CodVariante = v.CodVariante,
                    Stock = v.Stock,
                    PrecioVenta = v.PrecioVenta,
                    Estado = v.EstadoRegistro,
                    Talle = v.Talle != null ? v.Talle.NombreTalle : "Sin Talle",
                    ProductoColor = v.ProductoColor != null && v.ProductoColor.Producto != null && v.ProductoColor.Color != null
                        ? $"{v.ProductoColor.Producto.NombreProducto} - {v.ProductoColor.Color.NombreColor}"
                        : "Producto/Color no asignado"
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<VarianteMostrarDTO>> GetListaInactivos()
        {
            return await context.Variantes
                .Where(v => v.EstadoRegistro == EstadoRegistro.inactivo)
                .Include(v => v.ProductoColor)
                    .ThenInclude(pc => pc!.Producto)
                .Include(v => v.ProductoColor)
                    .ThenInclude(pc => pc!.Color)
                .Include(v => v.Talle)
                .Select(v => new VarianteMostrarDTO
                {
                    Id = v.Id,

                    // CORRECCIÓN CLAVE: Navegación profunda en las relaciones para extraer el ID correcto
                    ProductoId = (v.ProductoColor != null && v.ProductoColor.Producto != null)
                 ? v.ProductoColor.Producto.Id
                 : 0,

                    CodVariante = v.CodVariante,
                    Stock = v.Stock,
                    PrecioVenta = v.PrecioVenta,
                    Estado = v.EstadoRegistro,
                    Talle = v.Talle != null ? v.Talle.NombreTalle : "Sin Talle",
                    ProductoColor = v.ProductoColor != null && v.ProductoColor.Producto != null && v.ProductoColor.Color != null
        ? $"{v.ProductoColor.Producto.NombreProducto} - {v.ProductoColor.Color.NombreColor}"
        : "Producto/Color no asignado"
                })
                .ToListAsync();
        }



        public async Task<bool> Editar(int id, VarianteCrearDTO dto)
        {

            var registroExistente = await context.Variantes
                .FirstOrDefaultAsync(v => v.Id == id);

            if (registroExistente == null) return false;

            // Validar que el ProductoColorId proporcionado exista en la tabla ProductosColores
            var existeProductoColor = await context.ProductosColores
                .AnyAsync(pc => pc.Id == dto.ProductoColorId);
            if (!existeProductoColor) return false;

            registroExistente.CodVariante = dto.CodVariante;
            registroExistente.Stock = dto.Stock;
            registroExistente.PrecioVenta = dto.PrecioVenta;
            registroExistente.EstadoRegistro = dto.Estado;
            registroExistente.TalleId = dto.TalleId;
            registroExistente.ProductoColorId = dto.ProductoColorId;

            await context.SaveChangesAsync();
            return true;
        }




    }
}

