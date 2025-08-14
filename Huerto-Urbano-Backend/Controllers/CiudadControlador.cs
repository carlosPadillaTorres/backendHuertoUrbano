using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Huerto_Urbano_Backend.Controllers
{
    public class CiudadControlador: ControllerBase
    {
        private readonly ContextoBdAdmin _context;// Cambiar tipo ContextAdmin a Context Employee para context

        public CiudadControlador(ContextoBdAdmin context) // Cambiar tipo ContextAdmin a Context Employee para context
        {
            _context = context;
        }

        [HttpGet]
        [Route("obtenerCiudades")]
        public  ActionResult<IEnumerable<Ciudad>> ObtenerCiudades([FromQuery] string? idEstado)
        {
            var ciudades =  string.IsNullOrWhiteSpace(idEstado)
                ? _context.Ciudad.Include(c => c.Estado).ToList()
                : _context.Ciudad.Include(c => c.Estado)
                   .Where(c => EF.Functions.Like(c.IdEstado, idEstado) 
                    ).ToList();
            return ciudades;
        }
    }
}
