using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Models;
using Huerto_Urbano_Backend.Recursos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Huerto_Urbano_Backend.Controllers
{
    public class ClienteControlador : ControllerBase
    {
        private readonly ContextoBdAdmin _contextClient; // Debe ser cambiado el tipo => ContextoBdApp _contextClient;
        private readonly ContextoBdAdmin _contextAdmin;
        private readonly ContextoBdEmployee _contextEmpleado;

        public ClienteControlador(ContextoBdApp contextClient, ContextoBdAdmin contextAdmin, ContextoBdEmployee contextEmp)
        {
            _contextClient = contextAdmin; // Debe usarse conextClient. Modificar después de corregir permisos en la BD
            _contextAdmin = contextAdmin;
            _contextEmpleado = contextEmp;
        }

        [HttpGet]
        [Route("obtenerClientes")]
        public ActionResult<IEnumerable<Cliente>> ObtenerClientes([FromQuery] string? filtro)
        {
            var clientes = string.IsNullOrWhiteSpace(filtro)
                ? _contextAdmin.Cliente.Include(c => c.Persona).Include(p =>p.Persona.Domicilio).Include(p => p.Usuario).ToList()
                : _contextAdmin.Cliente.Include(c => c.Persona).Include(p => p.Persona.Domicilio)
                    .Where(c => EF.Functions.Like(c.Persona.Nombre, $"%{filtro.Trim()}%") ||
                                EF.Functions.Like(c.Persona.ApPaterno, $"%{filtro.Trim()}%") ||
                                EF.Functions.Like(c.Persona.ApMaterno, $"%{filtro.Trim()}%")
                    ).ToList();
            return clientes;
        }


        [HttpPost]
        [Route("registrarCliente")]
        public ActionResult<Cliente> RegistrarCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                Console.WriteLine("La estructura del objeto recibido (cliente) es incorrecta");
                return BadRequest("La estructura del objeto recibido (cliente) es incorrecta");
            }
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new {
                        Campo = ms.Key,
                        Mensajes = ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    });
                Console.WriteLine("Model Exception: " + errores);

                return BadRequest(new
                {
                    message = "Errores de validación",
                    errors = errores
                });
            }

            try
            {
                // Evitar que EF intente interpretar la expresión completa
                var nombreUsuarioIngresado = cliente.Usuario.NombreUsuario;

                var nombreUsuario = _contextClient.Usuario
                    .Where(u => u.NombreUsuario == nombreUsuarioIngresado)
                    .Select(u => u.NombreUsuario)
                    .FirstOrDefault();

                if (nombreUsuario != null)
                {
                    Console.WriteLine("El nombre de usuario ya existe: " + nombreUsuario);
                    return BadRequest(new { mensaje = "El nombre de usuario ya existe. Por favor, elige otro nombre de usuario." });
                }
                cliente.Usuario.Contrasenia = CifradoHash.Cifrar(cliente.Usuario.Contrasenia);
                _contextClient.Cliente.Add(cliente);
                _contextClient.SaveChanges();
                return Ok(new { mensaje = "Cuenta creada con éxito" });
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al registrar el cliente: " + e);
                return BadRequest("Error al registrar el usuario: " + e.Message);
            }
        }


        [HttpGet]
        [Route("obtenerCliente")]
        public IActionResult ObtenerCliente([FromQuery] int id)
        {
            var clienteEncontrado = _contextAdmin.Cliente
                .Include(p => p.Persona)
                .Include(p => p.Usuario)
                .Include(p => p.Persona.Domicilio)
                //.Include(p => p.Persona.Domicilio.Ciudad)
                //.Include(p => p.Persona.Domicilio.Ciudad.Estado)
                .FirstOrDefault(p => p.IdCliente == id);
            if (clienteEncontrado == null)
                return NotFound("Cliente no encontrado");

            clienteEncontrado.Usuario = Usuario.OcultarInfoSensible(clienteEncontrado.Usuario);
            return Ok(clienteEncontrado);
        }

        [HttpGet]
        [Route("obtenerContactoCliente")]
        public IActionResult ObtenerContactoCliente([FromQuery] int id) //Consultado para empleado
        {
            var clienteEncontrado = _contextEmpleado.Cliente
                .Include(p => p.Persona).Include(p => p.Persona.Domicilio)
                .FirstOrDefault(p => p.IdCliente == id);
            if (clienteEncontrado == null)
            {
                return NotFound("Cliente no encontrado");
            }
            clienteEncontrado.Persona.FechaNacimiento = new System.DateTime();// Ocultar fecha de nacimiento
            return Ok(clienteEncontrado);
        }

        [HttpPut]
        [Route("actualizarCliente")]
        public IActionResult ActualizarCliente([FromBody] ClienteDto cliente)
        {
            if (cliente == null)  return BadRequest("Datos del cliente no proporcionados.");

            var clienteEncontrado = _contextClient.Cliente
                .Include(c => c.Persona)
                .Include(c => c.Persona.Domicilio)
                .FirstOrDefault(p => p.IdCliente == cliente.IdCliente);
            if (clienteEncontrado == null) return NotFound("Cliente no encontrado");

            Console.WriteLine("cliente: " + clienteEncontrado);

            clienteEncontrado = ClienteDto.CrearClienteActualizacion(cliente, clienteEncontrado);
            //cliente.Persona.IdPersona = clienteEncontrado.IdPersona;

            _contextClient.Cliente.Update(clienteEncontrado);
            _contextClient.SaveChanges();
            return Ok();
        }

        [HttpPatch]
        [Route("eliminarCliente")]
        public IActionResult EliminarCliente([FromQuery] int id)
        {
            var cliente = _contextAdmin.Cliente.Include(p => p.Usuario).FirstOrDefault(p => p.IdCliente == id);
            if (cliente == null) return NotFound("Cliente no encontrado");

            cliente.Usuario.Estatus = false;
                _contextClient.SaveChanges();
                return Ok();
        }

        [HttpPatch]
        [Route("reactivarCliente")]
        public IActionResult ReactivarCliente([FromQuery] int id)
        {
            var cliente = _contextAdmin.Cliente.Include(p => p.Usuario).FirstOrDefault(p => p.IdCliente == id);
            if (cliente == null) return NotFound("Cliente no encontrado");

            cliente.Usuario.Estatus = true;
            _contextClient.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("cambioContraseniaClien")]
        public IActionResult CambioContraseniaClie([FromQuery] CambioContraseniaUsuario credencialCliente)
        {
            var clienteEncontrado = _contextClient.Cliente
                .Include(c =>c.Usuario)
                .FirstOrDefault(p => p.IdCliente == credencialCliente.Id);
            if (clienteEncontrado == null) return NotFound("Cliente no encontrado");

            //if (clienteEncontrado.Usuario.Contrasenia != credencialCliente.viejaContrasenia){ // Es por si tenemos que cifrarContraseña sin tener la password anterior
            if (clienteEncontrado.Usuario.Contrasenia != CifradoHash.Cifrar(credencialCliente.viejaContrasenia)){
                return BadRequest("La contraseña actual no coincide.");
            }
            clienteEncontrado.Usuario.Contrasenia = CifradoHash.Cifrar( credencialCliente.nuevaContrasenia);
            _contextClient.SaveChanges();
            return Ok();
        }

        [HttpPatch]
        [Route("TokenRecuperacion")]
        public IActionResult TokenRecuperacion([FromQuery] String correoCliente)
        {
            var cliente = _contextClient.Cliente.FirstOrDefault(p => p.Persona.Email == correoCliente);
            if (cliente == null) return NotFound("Correo no encontrado");
      
            //cliente.Usuario.tokenRecuperacion = '';
            _contextClient.SaveChanges();
            return Ok(new { message = "Se ha enviado el token de cambio de contrasenia." });
        }
    }

}
