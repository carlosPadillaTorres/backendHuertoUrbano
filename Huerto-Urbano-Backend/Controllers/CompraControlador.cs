using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Huerto_Urbano_Backend.Controllers
{
    [ApiController]
    [Route("Compra")]
    public class CompraControlador : ControllerBase
    {
        private readonly ContextoBdAdmin _contexto;
        public CompraControlador(ContextoBdAdmin contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        [Authorize]
        [Route("registrarCompra")]
        public async Task<IActionResult> RegistrarCompra([FromBody] RegistrarCompraDto compraDto)
        {
            var idUsuarioClaim = User.FindFirstValue("idUsuario");
            if (string.IsNullOrEmpty(idUsuarioClaim))
            {
                Console.WriteLine("No se pudo obtener el idUsuario del token");
                return Unauthorized(new { mensaje = "No se pudo obtener el idUsuario del token." });
            }
            int idUsuario = int.Parse(idUsuarioClaim);

            // 2️⃣ Obtener idEmpleado desde el idUsuario
            var empelado = _contexto.Empleado.FirstOrDefault(c => c.IdUsuario == idUsuario);
            if (empelado == null)
            {
                return NotFound(new { mensaje= "No se encontró el empleado asociado al usuario." });
            }
            Console.WriteLine("idEMp: "+empelado.IdEmpleado.ToString());

            // Validar proveedor
            var proveedor = await _contexto.Set<Proveedor>().FindAsync(compraDto.ProveedorId);
            if (proveedor == null) {
                Console.WriteLine("Proveedor no encotrado");
                return BadRequest(new { mensaje = "Proveedor no encontrado" });
            }
            // Crear compra
            var compra = new ComprasRealizadas
            {
                IdProveedor = compraDto.ProveedorId,
                Fecha = compraDto.FechaCompra,
                NumeroOrden = Guid.NewGuid().ToString().Substring(0, 11),
                Estatus = true,
                Precio = 0 ,// Se calculará abajo
                idEmpleado= empelado.IdEmpleado
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


                var producto =  _contexto.Producto
                    .FirstOrDefault(p => p.IdProducto == detalle.ProductoId);
                producto.CantidadTotal = producto.CantidadTotal + detalle.Cantidad;
                _contexto.Producto.Update(producto);


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
             _contexto.SaveChanges();

            return Ok(new { compra.IdComprasRealizadas });
        }
    }
}
