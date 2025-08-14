using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Huerto_Urbano_Backend.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class UsuarioControlador : ControllerBase
        {
            private readonly ContextoBdApp _context;

            public UsuarioControlador(ContextoBdApp context)
            {
                _context = context;
            }

        
        [HttpGet]
        [Route("/obtenerUsuarios")]
        public ActionResult<IEnumerable<Usuario>> Get()
        {
            return _context.Usuario.ToList();
        }

        [HttpPost]
         public ActionResult<Usuario> RegistrarUsuario(Usuario usuario)
         {
                _context.Usuario.Add(usuario);
                _context.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = usuario.IdUsuario }, usuario);
          }
        }
    
}
