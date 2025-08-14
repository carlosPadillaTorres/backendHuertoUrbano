using Huerto_Urbano_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Dto
{
    public class PersonaDto
    {
        [StringLength(50)]
        public string ApPaterno { get; set; }

        [StringLength(50)]
        public string ApMaterno { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(10, MinimumLength = 10)]
        public string Telefono { get; set; }

        [Column(TypeName = "char(1)")]
        public char Genero { get; set; } // H, M, O
        public int IdDomicilio { get; set; }
        public DomicilioDto Domicilio { get; set; }


        public static Persona InicializarPersona(PersonaDto per)
        {
            return new Persona
            {
                //IdPersona = 0,
                ApPaterno = per.ApPaterno,
                ApMaterno = per.ApMaterno,
                Nombre = per.Nombre,
                Telefono = per.Telefono,
                Email = per.Email,
                FechaNacimiento = per.FechaNacimiento,
                Genero = per.Genero,
                IdDomicilio= per.IdDomicilio,
                Domicilio = DomicilioDto.InicializarDomicilio(per.Domicilio)

            };
        }
    }
    
}
