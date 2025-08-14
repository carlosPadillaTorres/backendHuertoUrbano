using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Huerto_Urbano_Backend.Controllers
{
    [Route("Ventas")]
    public class VentaControlador: ControllerBase
    {

        private readonly ContextoBdAdmin _contextAdmin;
        private readonly ContextoBdAdmin _contextClien;// Cambiar tipo ContextAdmin a Context Employee para context


        public VentaControlador(ContextoBdAdmin context, ContextoBdAdmin contextoAdmin) // Cambiar tipo ContextAdmin a Context Employee para context
        {
            _contextClien = context;
            _contextAdmin = contextoAdmin;
        }

        [HttpGet]
        [Route("obtenerVentas")]
        public ActionResult<IEnumerable<DetalleVenta>> ObtenerVentas([FromQuery] string? filtro)
        {
            var query = _contextAdmin.DetalleVenta
                .Include(v => v.Venta)
                .Include(v => v.Producto)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                try
                {
                    var fechas = filtro.Split('|');

                    if (fechas.Length == 2 &&
                        DateTime.TryParse(fechas[0], out DateTime fechaInicio) &&
                        DateTime.TryParse(fechas[1], out DateTime fechaFin))
                    {
                        // Incluir el día completo del fin
                        fechaFin = fechaFin.Date.AddDays(1).AddTicks(-1);
                        query = query.Where(v =>
                            v.Venta.FechaVenta >= fechaInicio &&
                            v.Venta.FechaVenta <= fechaFin
                        );
                    }
                    else
                    {
                        return BadRequest("El parámetro 'filtro' debe tener el formato 'yyyy-MM-dd|yyyy-MM-dd'.");
                    }
                }
                catch (Exception e)
                {
                    return BadRequest("Error al procesar el filtro: " + e.Message);
                }
            }

            var ventas = query.ToList();
            return Ok(ventas);
        }



        
        [HttpGet]
        [Route("obtenerVentasCliente")]
        [Authorize]
        public IActionResult ObtenerVentasCliente()
        {
            try
            {
                // 1️⃣ Obtener idUsuario desde token
                var idUsuarioClaim = User.FindFirstValue("idUsuario");
                if (string.IsNullOrEmpty(idUsuarioClaim))
                {
                    return Unauthorized("No se pudo obtener el idUsuario del token.");
                }
                int idUsuario = int.Parse(idUsuarioClaim);

                // 2️⃣ Obtener cliente por idUsuario
                var cliente = _contextClien.Cliente.FirstOrDefault(c => c.IdUsuario == idUsuario);
                if (cliente == null)
                {
                    return NotFound("No se encontró un cliente asociado al usuario.");
                }

                int idCliente = cliente.IdCliente;

                // 3️⃣ Obtener ventas y detalles
                    var ventas = _contextClien.Venta
                         .Where(v => v.IdCliente == idCliente)
                         .OrderByDescending(v => v.FechaVenta)
                         .Select(v => new VentaConDetallesDto
                         {
                             IdVenta = v.IdVenta,
                             FechaVenta = v.FechaVenta,
                             Total = v.Total,
                             Estatus = v.Estatus,
                             Detalles = _contextClien.DetalleVenta
                                 .Where(d => d.IdVenta == v.IdVenta)
                                 .Select(d => new DetalleVentaDto
                                 {
                                     IdDetalleVenta = d.IdDetalleVenta,
                                     IdProducto = d.IdProducto,
                                     NombreProducto = d.Producto.NombreProducto,
                                     Marca = d.Producto.Marca,
                                     Cantidad = d.Cantidad,
                                     PrecioUnitario = d.PrecioUnitario
                                 }).ToList()
                         })
                         .ToList();
                Console.WriteLine(ventas[0].Total);

                return Ok(ventas);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener ventas: " + ex);
                return BadRequest("Error al obtener las ventas: " + ex.Message);
            }
        }


        [HttpPost]
        [Route("registrarVenta")]
        [Authorize(Roles="CLIE")] 
        public IActionResult RegistrarVenta([FromBody] RegistrarVentaDto ventaDto)
        {
            using var transaction = _contextClien.Database.BeginTransaction();

            try
            {
                // 1️⃣ Obtener idUsuario del token
                var idUsuarioClaim = User.FindFirstValue("idUsuario");
                if (string.IsNullOrEmpty(idUsuarioClaim))
                {
                    return Unauthorized("No se pudo obtener el idUsuario del token.");
                }
                int idUsuario = int.Parse(idUsuarioClaim);

                // 2️⃣ Obtener idCliente desde el idUsuario
                var cliente = _contextClien.Cliente.FirstOrDefault(c => c.IdUsuario == idUsuario);
                if (cliente == null)
                {
                    return NotFound("No se encontró el cliente asociado al usuario.");
                }

                // 3️⃣ Crear el registro de la venta
                var nuevaVenta = new Venta
                {
                    IdCliente = cliente.IdCliente,
                    FechaVenta = DateTime.Now,
                    Total = ventaDto.Total,
                    Estatus = true
                };

                _contextClien.Venta.Add(nuevaVenta);
                _contextClien.SaveChanges();

                // 4️⃣ Registrar cada detalle y lote
                foreach (var prod in ventaDto.Productos)
                {
                    // Insertar en detalleVenta
                    var detalle = new DetalleVenta
                    {
                        IdVenta = nuevaVenta.IdVenta,
                        IdProducto = prod.IdProducto,
                        Cantidad = prod.Cantidad,
                        PrecioUnitario = prod.PrecioUnitario
                    };
                    _contextClien.DetalleVenta.Add(detalle);

                    // Insertar en loteProducto
                    /*var lote = new LoteProducto
                    {
                        IdProducto = prod.IdProducto,
                        FechaIngreso = DateTime.Now,
                        CantidadLote = prod.Cantidad,
                        CostoTotal = prod.PrecioUnitario * prod.Cantidad,
                        Existencia = prod.Cantidad
                    };
                    _contextClien.LoteProducto.Add(lote);*/
                }

                // 5️⃣ Guardar cambios
                _contextClien.SaveChanges();

                // 6️⃣ Confirmar transacción
                transaction.Commit();

                return Ok(new
                {
                    message = "Venta registrada correctamente",
                    idVenta = nuevaVenta.IdVenta
                });
            }
            catch (Exception ex)
            {
                // Si algo falla, revertir transacción
                transaction.Rollback();
                Console.WriteLine("Error al registrar venta: " + ex);
                return BadRequest("Error al registrar la venta: " + ex.Message);
            }
        }

    }
}
