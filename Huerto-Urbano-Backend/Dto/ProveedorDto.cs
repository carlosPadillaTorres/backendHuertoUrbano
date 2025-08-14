using Huerto_Urbano_Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Huerto_Urbano_Backend.Dto
{
    public class ProveedorDto
    {
        [Required, StringLength(100)]
        public required string Empresa { get; set; }
        [Required, StringLength(10)]
        public required string Telefono { get; set; }
        [Required, EmailAddress, StringLength(100)]
        public required string Email { get; set; }
        [Required, StringLength(12)]
        public required string Rfc { get; set; }
        public Domicilio Domicilio { get; set; }
    }
}
