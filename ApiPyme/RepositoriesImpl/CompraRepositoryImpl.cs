﻿using ApiPyme.Context;
using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Globalization;

namespace ApiPyme.RepositoriesImpl
{
    public class CompraRepositoryImpl : ICompraRepository
    {
        private readonly AppDbContext _context;
        private readonly IProductoRepository _productoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        public CompraRepositoryImpl(AppDbContext context, IProductoRepository productoRepository, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _productoRepository = productoRepository;
            _usuarioRepository = usuarioRepository;
        }
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<CompraDto>> getAllCompras(int page, int size, string search)
        {
            var query = _context.Compras.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.NumeroCompra.Contains(search));
            }

            // Asegura que los valores de paginación sean válidos
            if (page < 1)
                page = 1;

            if (size < 1)
                size = 10; // Establecer un tamaño predeterminado si no es válido

            // Calcula el número de elementos a saltar
            int skip = (page - 1) * size;

            // Total de elementos antes de aplicar paginación
            int totalCount = await query.CountAsync();

            // Obtener los elementos de la página actual
            var items = await query.Skip(skip).Take(size).ToListAsync();

            var compraDto = items.Select(c => new CompraDto
            {
                IdCompra = c.IdCompra + "",
                NumeroCompra = c.NumeroCompra,
                FechaCompra = c.FechaCompra.ToString(),
                TipoComprobante = c.TipoComprobante,
                Iva = c.Iva + "",
                TotalCompra = c.TotalCompra.ToString("F2")
            });

            // Retornar el resultado paginado
            return new PagedResult<CompraDto>
            {
                TotalCount = totalCount,
                Items = compraDto
            };
        }

        public async Task<CompraDto> GetCompra(int id)
        {
            var compra = await _context.Compras
        .Include(c => c.detallesCompra)
        .ThenInclude(d => d.producto) // Incluir la relación con Producto
        .FirstOrDefaultAsync(c => c.IdCompra == id);

            if (compra == null)
            {
                return null;
            }

            // Leer y convertir el archivo PDF a Base64
            string? fileBase64 = null;
            if (!string.IsNullOrEmpty(compra.PathPdf) && File.Exists(compra.PathPdf))
            {
                var fileBytes = await File.ReadAllBytesAsync(compra.PathPdf);
                fileBase64 = Convert.ToBase64String(fileBytes);
            }

            // Crear el DTO
            var compraDto = new CompraDto
            {
                IdCompra = compra.IdCompra.ToString(),
                NumeroCompra = compra.NumeroCompra,
                TipoComprobante = compra.TipoComprobante,
                FechaCompra = compra.FechaCompra.ToString("yyyy-MM-dd"), // Formato de fecha
                TotalCompra = compra.TotalCompra.ToString("F2"), // Formato decimal con dos decimales
                Iva = compra.Iva.ToString(),
                IdUsuarioProveedor = compra.IdUsuarioProveedor?.ToString(),
                IdUsuarioComerciante = compra.IdUsuarioComerciante?.ToString(),
                FileBase64 = fileBase64, // Asignar el archivo en Base64
                DetalleCompras = compra.detallesCompra?.Select(d => new DetalleCompraDto
                {
                    IdDetalleCompra = d.IdDetalleCompra.ToString(),
                    CantidadInicial = d.CantidadInicial.ToString(),
                    IdProducto = d.IdProducto?.ToString(),
                    NombreProducto = d.producto?.NombreProducto, // Incluir el nombre del producto
                    PrecioUnitario = d.PrecioUnitario.ToString("F2") // Formato decimal con dos decimales
                }).ToList()
            };

            return compraDto;
        }

        public async Task<bool> Save(CompraDto compraDto)
        {
            try
            {
                // busco el id de usuario proveedor y comerciante
                var usuarioProveedor = await _usuarioRepository.GetUsuario(Int32.Parse(compraDto.IdUsuarioProveedor));
                var usuarioComerciante = await _usuarioRepository.GetUsuario(Int32.Parse(compraDto.IdUsuarioComerciante));

                //  guardo la cabecera
                if (compraDto.FileBase64 == null)
                {
                    throw new Exception("PDF requerido");
                }

                string filePath = GuardarArchivoPdf(compraDto.FileBase64);

                Compra compra = new Compra();
                compra.NumeroCompra = compraDto.NumeroCompra;
                compra.Iva = Int32.Parse(compraDto.Iva);
                compra.Observacion = compraDto.Observacion;
                compra.TipoComprobante = compraDto.TipoComprobante;
                compra.usuarioProveedor = usuarioProveedor;
                compra.usuarioComerciante = usuarioComerciante;
                compra.FechaCompra = DateTime.ParseExact(compraDto.FechaCompra, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                compra.TotalCompra = decimal.Parse(compraDto.TotalCompra, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                compra.PathPdf = filePath;

                // primero se verifica los productos si tienen IdProducto
                await _context.Compras.AddAsync(compra);
                await _context.SaveChangesAsync();

                foreach (var item in compraDto.DetalleCompras)
                {
                    if (!string.IsNullOrEmpty(item.IdProducto))
                    {
                        // se busca el producto y se actualiza el total}
                        Producto productoUpdate = await _productoRepository.GetProducto(Int32.Parse(item.IdProducto));
                        var nuevoStock = (productoUpdate.Stock + Int32.Parse(item.CantidadInicial));
                        productoUpdate.Stock = nuevoStock;
                        await _productoRepository.UpdateProductoCompra(productoUpdate);

                        // en el caso de que exista el producto solo se actualiza y 
                        // se guarda el detalle con el id 
                        DetalleCompra detalle = new DetalleCompra();
                        detalle.CantidadInicial = Int32.Parse(item.CantidadInicial);
                        detalle.producto = productoUpdate;
                        detalle.compra = compra;
                        detalle.PrecioUnitario = decimal.Parse(item.PrecioUnitario, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture); 
                        await _context.DetalleCompras.AddAsync(detalle);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // se crea un nuevo producto en caso que el id este en null
                        ProductoDto productoNew = new ProductoDto();
                        productoNew.NombreProducto = item.NombreProducto;
                        productoNew.Descripcion = item.Descripcion;
                        productoNew.Stock = item.CantidadInicial;
                        productoNew.Precio = item.PrecioUnitario;
                        productoNew.IdProveedor = compraDto.IdUsuarioProveedor;

                        var productoCreate = await _productoRepository.SaveProductoCompra(productoNew);

                        // en el caso de que se cree el producto se guarda y se recupera el id
                        // se guarda el detalle con el id 
                        DetalleCompra detalle = new DetalleCompra();
                        detalle.CantidadInicial = Int32.Parse(item.CantidadInicial);
                        detalle.producto = productoCreate;
                        detalle.compra = compra;
                        detalle.PrecioUnitario = decimal.Parse(item.PrecioUnitario, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                        await _context.DetalleCompras.AddAsync(detalle);
                        await _context.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<bool> Update(Compra compra)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ComprobanteResumenDto>> GetResumenCompras()
        {
            // Obtén la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Determina la fecha de inicio del mes actual (primer día del mes)
            DateTime inicioDelMes = new DateTime(fechaActual.Year, fechaActual.Month, 1);

            // Consulta usando LINQ
            var resultados = await _context.Compras
                .Where(c => c.FechaCompra >= inicioDelMes && c.FechaCompra <= fechaActual) // Filtrar por mes y tipo de transacción
                .GroupBy(c => c.TipoComprobante)
                .Select(g => new ComprobanteResumenDto
                {
                    TipoComprobante = g.Key,
                    Total = g.Sum(c => (decimal)c.TotalCompra) // Asegurar conversión a decimal si es necesario
                })
                .ToListAsync();

            return resultados;
        }

        private string GuardarArchivoPdf(string fileBase64)
        {
            try
            {
                var base64Data = fileBase64.Split(",")[1]; // Remover encabezado Base64
                var fileBytes = Convert.FromBase64String(base64Data);

                // Crear la carpeta si no existe
                if (!Directory.Exists(_uploadFolderPath))
                {
                    Directory.CreateDirectory(_uploadFolderPath);
                }

                var filePath = Path.Combine(_uploadFolderPath, $"{Guid.NewGuid()}.pdf");
                File.WriteAllBytes(filePath, fileBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el archivo PDF: {ex.Message}", ex);
            }
        }



    }
}
