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
    public class ProductoRepositorio : Repositorio<Producto>, IRepositorio<Producto>, IProductoRepositorio
    {
        private readonly AppDbContext context;

        public ProductoRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<ProductoMostrarDTO>> GetListaActivos()
        {
            return await context.Productos
                .Where(pc => pc.EstadoRegistro == EstadoRegistro.activo)
                .Include(pc => pc.Proveedor)
                .Select(pc => new ProductoMostrarDTO
                {

                    // EF Core se encarga de buscar el nombre a través de la relación
                    Id = pc.Id,
                    Nombre = pc.NombreProducto,
                    Descripcion = pc.DescripcionProducto,
                    Marca = pc.MarcaProducto,
                    Proveedor = pc.Proveedor!.RazonSocialProveedores,
                    Estado = pc.EstadoRegistro
                })
                .ToListAsync();
        }

        public async Task<List<ProductoMostrarDTO>> GetListaInactivos()
        {
            return await context.Productos.Where(pc => pc.EstadoRegistro == EstadoRegistro.inactivo)
                .Include(pc => pc.Proveedor)
                .Select(pc => new ProductoMostrarDTO
                {

                    // EF Core se encarga de buscar el nombre a través de la relación
                    Id = pc.Id,
                    Nombre = pc.NombreProducto,
                    Descripcion = pc.DescripcionProducto,
                    Marca = pc.MarcaProducto,
                    Proveedor = pc.Proveedor!.RazonSocialProveedores,
                    Estado = pc.EstadoRegistro
                })
                .ToListAsync();
        }



        public async Task<bool> Editar(int id, ProductoCrearDTO dto)
        {

            var registroExistente = await context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (registroExistente == null) return false;


            registroExistente.NombreProducto = dto.Nombre;
            registroExistente.DescripcionProducto = dto.Descripcion;
            registroExistente.MarcaProducto = dto.Marca;
            registroExistente.ProveedorId = dto.ProveedorId;
            registroExistente.EstadoRegistro = dto.Estado;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductoMostrarDTO?> ObtenerById(int id)
        {
            return await context.Productos
                .Where(p => p.Id == id)
                .Select(p => new ProductoMostrarDTO
                {
                    Id = p.Id,
                    Nombre = p.NombreProducto,
                    Descripcion = p.DescripcionProducto,
                    Marca = p.MarcaProducto,
                    Proveedor = p.Proveedor != null ? p.Proveedor.RazonSocialProveedores : "Sin Proveedor",
                    Estado = p.EstadoRegistro
                })
                .FirstOrDefaultAsync();
        }


    }
}

