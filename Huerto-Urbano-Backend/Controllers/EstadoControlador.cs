using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Huerto_Urbano_Backend.Controllers
{
    public class EstadoControlador: ControllerBase
    {
        private readonly ContextoBdAdmin _context;// Cambiar tipo ContextAdmin a Context Cliente para context

    public EstadoControlador(ContextoBdAdmin context) // Cambiar tipo ContextAdmin a Context Employee para context
    {
        _context = context;
    }

    [HttpGet]
    [Route("obtenerEstados")]
    public ActionResult<IEnumerable<Estado>> ObtenerEstados()
    {       
            var estados = _context.Estado.ToList();
        return estados;
    }
}
        
}



