using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Recursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace Huerto_Urbano_Backend.Controllers
{

  

    [ApiController]
    public class AutenticacionControlador : ControllerBase
    {
        private readonly ContextoBdApp _context;

        public AutenticacionControlador(ContextoBdApp context)
        {
            _context = context;
        }

        [HttpPost("inicioSesion")]
        public IActionResult IniciarSesion(InicioUsuarioDto usuario)
        {
            try
            {
                usuario.Contrasenia = CifradoHash.Cifrar(usuario.Contrasenia); // Se cifra para la comparación en la BD
                var usuarioEncontrado = _context.Usuario
                    .FirstOrDefault(u => u.NombreUsuario == usuario.NombreUsuario && u.Contrasenia == usuario.Contrasenia);

                //Console.WriteLine("Credenciales usuari2: " + usuarioEncontrado.Contrasenia + "  UN: " + usuarioEncontrado.NombreUsuario);


                if (usuarioEncontrado == null) return Unauthorized();

                Console.WriteLine("Antes de tokenizar");
                var token = ServicioToken.GenerateToken(usuarioEncontrado.IdUsuario, usuarioEncontrado.NombreUsuario, usuarioEncontrado.Rol);
                Console.WriteLine("Después de tokenizar");

                usuarioEncontrado.Token = token;
                _context.Usuario.Update(usuarioEncontrado);
                _context.SaveChanges();
              
                return Ok(new { token });
            }
            catch (Exception e) {
                Console.WriteLine("ERROR:");
                Console.WriteLine(e);
                return Conflict("ERROR: "+e);
            }
        }


        /*[HttpGet("cerrarSesion")]
        public IActionResult CerrarSesion()
        {
            var header = Request.Headers["Authorization"].ToString();
            Console.WriteLine("Header de autorización: " + header);

            UsuarioLogeadoDto datosUsuario = DecodificadorToken.DecodificarToken(
                    Request.Headers["Authorization"].ToString() // Token
                );
            if (datosUsuario == null) { 
                return Unauthorized("No se pudo obtener el token de la solicitud.");
            }


            var usuarioEncontrado = _context.Usuario
                .FirstOrDefault(u => u.IdUsuario == datosUsuario.idUsuario);

            if (usuarioEncontrado == null) {
                return BadRequest("Algo salió mal");
            }

            usuarioEncontrado.Token = "";
            _context.Usuario.Update(usuarioEncontrado);
            _context.SaveChanges();

            return Ok(new { mensaje = "Sesión cerrada con éxito" });
        }*/


        [HttpGet("cerrarSesion")]
        public IActionResult CerrarSesion()
        {
            Console.WriteLine("Cerrando sesión...");

            var idUsuario = User.FindFirst("idUsuario")?.Value;

            if (idUsuario == null)
            {
                return Unauthorized("No se pudo obtener el token de la solicitud.");
            }


            var usuarioEncontrado = _context.Usuario
                .FirstOrDefault(u => u.IdUsuario == int.Parse(idUsuario));

            if (usuarioEncontrado == null)
            {
                return BadRequest("Algo salió mal");
            }

            usuarioEncontrado.Token = "";
            _context.Usuario.Update(usuarioEncontrado);
            _context.SaveChanges();

            return Ok(new { mensaje = "Sesión cerrada con éxito" });
        }

    }
}
