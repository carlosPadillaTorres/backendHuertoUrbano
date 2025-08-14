using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;
using System.Security.Claims;

namespace Huerto_Urbano_Backend.Controllers
{
    [Route("Proveedor")]
    public class ProveedorControlador : ControllerBase
    {
        private readonly ContextoBdAdmin _context;

        public ProveedorControlador(ContextoBdAdmin context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        [Route("obtenerProveedores")]
        public ActionResult<IEnumerable<Proveedor>> ObtenerProveedores([FromQuery] string? filtro)
        {
            var id = User.FindFirst("idUsuario")?.Value;
            var nombre = User.FindFirst("nombreUsuario")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            Console.WriteLine("id: " + id + "\n nombre: " + nombre+"\n rol: " + rol);

            var proveedores = string.IsNullOrWhiteSpace(filtro)
                ? _context.Proveedor.Include(p => p.Domicilio)
                                    .Include(p => p.Domicilio.Ciudad)
                                    .Include(p => p.Domicilio.Ciudad.Estado).ToList()
                : _context.Proveedor.Include(p => p.Domicilio)
                                    .Include(p => p.Domicilio.Ciudad)
                                    .Include(p => p.Domicilio.Ciudad.Estado)
                    .Where(p => EF.Functions.Like(p.Empresa, $"%{filtro.Trim()}%")).
                    ToList();
            return proveedores;
        }

        [HttpPost]
        [Route("registrarProveedor")]
        public ActionResult<Proveedor> RegistrarProveedor([FromBody] ProveedorDto proveedor)
        {
            Proveedor proveedorInit = new Proveedor(proveedor);

            Console.WriteLine("Proveedor a registrar: " + proveedor.ToString());
            
            _context.Proveedor.Add(proveedorInit);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RegistrarProveedor), new { id = proveedorInit.IdProveedor }, proveedor);
        }


        [HttpGet]
        [Route("obtenerProveedor")]
        public IActionResult ObtenerProveedor([FromQuery] int id)
        {
            var proveedorEncontrado = _context.Proveedor
                .Include(p => p.Domicilio)
                .FirstOrDefault(p => p.IdProveedor == id);
            if (proveedorEncontrado == null)
            {
                return NotFound("Proveedor no encontrado");
            }
            return Ok(proveedorEncontrado);
        }


        [HttpPut]
        [Route("actualizarProveedor")]
        public IActionResult ActualizarProveedor([FromBody]
        Proveedor proveedor)
        {
            try
            {
                if (proveedor == null)
                {
                    Console.WriteLine("La estructura del objeto recibido (proveedor) es incorrecta");
                    return BadRequest("La estructura del objeto recibido (proveedor) es incorrecta");
                }
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Model ERROR: " + ModelState);
                    return BadRequest(ModelState);
                }
                _context.Proveedor.Update(proveedor);
                _context.SaveChanges();
                return Ok("Proveedor actualizado correctamente");
                
            }
            catch (DbEntityValidationException ex){
                Console.WriteLine("Exception ERROR: " + ex);
                return BadRequest(ex);
            }
            catch (Exception e) {

                Console.WriteLine("Error inesperado: "+e);
                return BadRequest("Error inesperado: "+e);
            }
            
                ;
            
        }

        [HttpPatch]
        [Route("eliminarProveedor")]
        public IActionResult EliminarProveedor([FromQuery] int id)
        {
            var proveedor = _context.Proveedor.FirstOrDefault(p => p.IdProveedor == id);
            if (proveedor != null)
            {
                proveedor.Estatus = false; 
                _context.SaveChanges();
                return Ok("Proveedor desactivado");
            }
            else
            {
                return NotFound("Proveedor no encontrado");
            }
        }

        [HttpPatch]
        [Route("reactivarProveedor")]
        public IActionResult reactivarProveedor([FromQuery] int id)
        {
            var proveedor = _context.Proveedor.FirstOrDefault(p => p.IdProveedor == id);
            if (proveedor != null)
            {
                proveedor.Estatus = true;
                _context.SaveChanges();
                return Ok("Proveedor reactivado");
            }
            else
            {
                return NotFound("Proveedor no encontrado");
            }
        }
    }
}
