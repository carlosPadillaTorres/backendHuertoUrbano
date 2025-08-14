using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Models;
using Huerto_Urbano_Backend.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Huerto_Urbano_Backend.Controllers
{
    [ApiController]
    [Route("Compra")]
    public class CompraControlador : ControllerBase
    {
        private readonly ContextoBdApp _contexto;
        public CompraControlador(ContextoBdApp contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        [Route("registrarCompra")]
        public async Task<IActionResult> RegistrarCompra([FromBody] RegistrarCompraDto compraDto)
        {
            // Validar proveedor
            var proveedor = await _contexto.Set<Proveedor>().FindAsync(compraDto.ProveedorId);
            if (proveedor == null)
                return BadRequest("Proveedor no encontrado");

            // Crear compra
            var compra = new ComprasRealizadas
            {
                IdProveedor = compraDto.ProveedorId,
                Fecha = compraDto.FechaCompra,
                NumeroOrden = Guid.NewGuid().ToString().Substring(0, 11),
                Estatus = true,
                Precio = 0 // Se calculará abajo
            };
            _contexto.ComprasRealizadas.Add(compra);
            await _contexto.SaveChangesAsync();

            decimal totalCompra = 0;
            // Registrar detalles y lotes
            foreach (var detalle in compraDto.Detalles)
            {
                var detalleCompra = new DetalleCompra
                {
                    IdComprasRealizadas = compra.IdComprasRealizadas,
                    IdProducto = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                };
                _contexto.DetalleCompra.Add(detalleCompra);

                var lote = new LoteProducto
                {
                    IdProducto = detalle.ProductoId,
                    FechaIngreso = compraDto.FechaCompra,
                    CantidadLote = detalle.Cantidad,
                    CostoTotal = detalle.Cantidad * detalle.PrecioUnitario,
                    Existencia = detalle.Cantidad
                };
                _contexto.LoteProducto.Add(lote);
                totalCompra += detalle.Cantidad * detalle.PrecioUnitario;
            }
            compra.Precio = totalCompra;
            await _contexto.SaveChangesAsync();

            return Ok(new { compra.IdComprasRealizadas });
        }
    }
}
