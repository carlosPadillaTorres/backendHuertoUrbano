using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Huerto_Urbano_Backend.Controllers
{
    [ApiController]
    [Route("Producto")]
    public class ProductoControlador : ControllerBase
    {
        private readonly ContextoBdAdmin _context;

        public ProductoControlador(ContextoBdAdmin context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("obtenerProductos")]
        public ActionResult<IEnumerable<Producto>> ObtenerProductos([FromQuery] string? filtro)
        {
            var productos = string.IsNullOrWhiteSpace(filtro)
                ? _context.Producto.Include(p => p.Proveedor).ToList()
                : _context.Producto
                    .Where(p => EF.Functions.Like(p.NombreProducto, $"%{filtro.Trim()}%")).
                    Include(p => p.Proveedor).
                    ToList();

            return productos;
        }

        [HttpPost]
        [Route("registrarProducto")]
        public ActionResult<Proveedor> RegistrarProducto([FromBody]ProductoDto producto)
        {
            Producto productoInit = new Producto().InicializarProducto(producto);

            _context.Producto.Add(productoInit);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RegistrarProducto), new { id = productoInit.IdProducto }, productoInit);
        }

        
        [HttpGet]
        [Route("obtenerProducto")]
        public IActionResult ObtenerProducto([FromQuery]int id)
        {
            var productoEncotrado = _context.Producto.FirstOrDefault(p => p.IdProducto == id);
            if (productoEncotrado == null)
            {
                return NotFound("Producto no encontrado");
            }
            return Ok(productoEncotrado);
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        [Route("actualizarProducto")]
        public IActionResult ActualizarProducto( [FromBody] Producto producto)
        {
            _context.Producto.Update(producto);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpPatch]
        [Route("eliminarProducto")]
        public IActionResult EliminarProducto([FromQuery] int id)
        {
            var producto = _context.Producto.FirstOrDefault(p => p.IdProducto == id);
            if (producto != null)
            {
                producto.Estatus = false; // Cambia el estatus a falso en lugar de eliminar
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound("Producto no encontrado");
            }
        }
    }
}
