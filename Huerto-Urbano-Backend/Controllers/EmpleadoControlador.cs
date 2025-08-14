using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Models;
using Huerto_Urbano_Backend.Recursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;

namespace Huerto_Urbano_Backend.Controllers
{
    [Route("Empleado")]
    public class EmpleadoControlador: ControllerBase
    {
        private readonly ContextoBdAdmin _contextAdmin;
        private readonly ContextoBdAdmin _contextEmp;// Cambiar tipo ContextAdmin a Context Employee para context

        public EmpleadoControlador(ContextoBdAdmin context, ContextoBdAdmin contextoAdmin) // Cambiar tipo ContextAdmin a Context Employee para context
        {
            _contextEmp = context;
            _contextAdmin = contextoAdmin;
        }

        [HttpGet]
        [Route("obtenerEmpleados")]
        public ActionResult<IEnumerable<Empleado>> ObtenerEmpleados([FromQuery] string? filtro)
        {
            var empleados = string.IsNullOrWhiteSpace(filtro)
                ? _contextAdmin.Empleado.Include(e => e.Persona).Include(e => e.Usuario).Include(e => e.Persona.Domicilio)
                    .Include(e => e.Persona.Domicilio.Ciudad).Include(e => e.Persona.Domicilio.Ciudad.Estado).ToList()

                : _contextAdmin.Empleado.Include(e => e.Persona).Include(e => e.Usuario).
                     Include(e => e.Persona.Domicilio).Include(e => e.Persona.Domicilio.Ciudad).Include(e => e.Persona.Domicilio.Ciudad.Estado)
                        .Where(e => EF.Functions.Like(e.Persona.Nombre, $"%{filtro.Trim()}%") ||
                                EF.Functions.Like(e.Persona.ApPaterno, $"%{filtro.Trim()}%") ||
                                EF.Functions.Like(e.Persona.ApMaterno, $"%{filtro.Trim()}%")
                    ).ToList();
            return Ok(empleados);
        }

        [HttpPost]
        [Authorize (Roles = "ADMS")]
        [Route("registrarEmpleado")]
        public ActionResult<Empleado> RegistrarEmpleado([FromBody] EmpleadoDto empleado)
        {
            try
            {
                if (empleado == null)
                {
                    Console.WriteLine("La estructura del objeto recibido (empleado) es incorrecta");
                    return BadRequest("La estructura del objeto recibido (empleado) es incorrecta");
                }
                if (!ModelState.IsValid)
                {
                    var errores = ModelState
                        .Where(ms => ms.Value.Errors.Count > 0)
                        .Select(ms => new {
                            Campo = ms.Key,
                            Mensajes = ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        });
                    Console.WriteLine("Model Exception: "+errores);

                    return BadRequest(new
                    {
                        message = "Errores de validación",
                        errors = errores
                    });
                }

                Empleado empleadoInit = new Empleado(empleado);

                _contextAdmin.Empleado.Add(empleadoInit);
                _contextAdmin.SaveChanges();
                return CreatedAtAction(nameof(RegistrarEmpleado), new { id = empleadoInit.IdEmpleado }, empleado);

            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine("Exception ERROR: " + ex);
                return BadRequest(ex);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error inesperado: " + e);
                return BadRequest("Error inesperado: " + e);
            }
        }

        [HttpGet]
        [Route("obtenerEmpleado")]
        public IActionResult ObtenerEmpleado([FromQuery] int id)
        {
            var empleadoEncontrado = _contextEmp.Empleado
                .Include(p => p.Persona)
                .Include(e => e.Persona.Domicilio)
                //.Include(e => e.Persona.Domicilio.Ciudad)
                //.Include(e => e.Persona.Domicilio.Ciudad.Estado)
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.IdEmpleado == id);

            if (empleadoEncontrado == null)
                return NotFound("Empleado no encontrado");

            empleadoEncontrado.Usuario = Usuario.OcultarInfoSensible(empleadoEncontrado.Usuario);
            return Ok(empleadoEncontrado);
        }

        [HttpGet]
        [Route("obtenerContactoEmpleado")]
        public IActionResult ObtenerContactoEmpleado([FromQuery] int id)
        {
            var empleadoEncontrado = _contextEmp.Empleado
                .Include(p => p.Persona)
                .Include(e => e.Persona.Domicilio)
                //.Include(e => e.Persona.Domicilio.Ciudad)
                //.Include(e => e.Persona.Domicilio.Ciudad.Estado)
                .FirstOrDefault(p => p.IdEmpleado == id);
            if (empleadoEncontrado == null)
                return NotFound("Empleado no encontrado");
            empleadoEncontrado.Persona.FechaNacimiento = new System.DateTime();// Ocultar fecha de nacimiento

            return Ok(empleadoEncontrado);
        }

        /*[HttpGet]
        [Route("obtenerContactoEmpleado")]
        public IActionResult ObtenerContactoEmpleado([FromQuery] int id)
        {

            var empEncontrado = _contextEmp.Empleado
                .Include(p => p.Persona).Include(p => p.Persona.Domicilio)
                .FirstOrDefault(p => p.IdEmpleado == id);
            if (empEncontrado == null)
                return NotFound("Empleado no encontrado");

            ContactoEmpleadoDto empDto = new ContactoEmpleadoDto(empEncontrado);
            empDto.Persona.FechaNacimiento = new System.DateTime();// Ocultar fecha de nacimiento
            return Ok(empDto);
        }*/

        [HttpPut]
        [Route("actualizarEmpleado")]
        public IActionResult ActualizarEmpleado([FromBody] Empleado empleado)
        {
            try
            {
                if (empleado == null)
                {
                    Console.WriteLine("La estructura del objeto recibido (empleado) es incorrecta");
                    return BadRequest("La estructura del objeto recibido (empleado) es incorrecta");
                }
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Model ERROR: " + ModelState);
                    return BadRequest(ModelState);
                }
                _contextAdmin.Empleado.Update(empleado);
                _contextAdmin.SaveChanges();
                return Ok("Empleado actualizado exitosamente");

            }catch (DbEntityValidationException ex)
            {
                Console.WriteLine("Exception ERROR: " + ex);
                return BadRequest(ex);
            }catch (Exception e)
            {
                Console.WriteLine("Error inesperado: " + e);
                return BadRequest("Error inesperado: " + e);
            }
        }

        [HttpPatch]
        [Route("eliminarEmpleado")]
        public IActionResult EliminarEmpleado([FromQuery] int id)
        {
            var empleado = _contextAdmin.Empleado.Include(e => e.Usuario).FirstOrDefault(p => p.IdEmpleado == id);
            if (empleado != null)
            {
                empleado.Usuario.Estatus = false;
                empleado.FechaRenuncia = DateTime.Today;
                _contextAdmin.SaveChanges();
                return Ok("Empleado desactivado ");
            }
                return NotFound("Empleado no encontrado");
            
        }

        [HttpPatch]
        [Route("reactivarEmpleado")]
        public IActionResult ReactivarEmpleado([FromQuery] int id)
        {
            var empleado = _contextAdmin.Empleado.Include(e => e.Usuario).FirstOrDefault(p => p.IdEmpleado == id);
            if (empleado != null)
            {
                empleado.Usuario.Estatus = true;
                _contextAdmin.SaveChanges();
                return Ok("Empleado recontratado");
            }
            return NotFound("Empleado no encontrado");

        }


        [HttpPost]
        [Route("cambioContraseniaEmp")]
        public IActionResult CambioContraseniaEmp([FromQuery] CambioContraseniaUsuario credencialCliente)
        {
            var empleadoEncontrado = _contextEmp.Empleado
                .Include(c => c.Usuario)
                .FirstOrDefault(p => p.IdEmpleado == credencialCliente.Id);

            if (empleadoEncontrado == null) return NotFound("Empleado no encontrado");


            if (empleadoEncontrado.Usuario.Contrasenia != CifradoHash.Cifrar(credencialCliente.viejaContrasenia))
                return BadRequest("La contraseña actual no coincide.");
            
            empleadoEncontrado.Usuario.Contrasenia = CifradoHash.Cifrar(credencialCliente.nuevaContrasenia);
            _contextEmp.SaveChanges();
            return Ok();
        }
    }
}
