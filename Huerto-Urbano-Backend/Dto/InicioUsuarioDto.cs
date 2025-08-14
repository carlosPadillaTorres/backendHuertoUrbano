using System.ComponentModel.DataAnnotations;

namespace Huerto_Urbano_Backend.Dto
{
    public class InicioUsuarioDto
    {
        [Required, StringLength(20)]
        public required string NombreUsuario { get; set; }

        [Required]
        public required string Contrasenia { get; set; }

    }
}
