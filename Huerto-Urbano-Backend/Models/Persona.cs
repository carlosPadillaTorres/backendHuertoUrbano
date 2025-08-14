using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{

    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Required]
        [StringLength(50)]
        public string ApPaterno { get; set; }

        [Required]
        [StringLength(50)]
        public string ApMaterno { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string Telefono { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public char Genero { get; set; } // H, M, O

        [Required]
        public int IdDomicilio { get; set; }

        [ForeignKey("IdDomicilio")]
        public virtual Domicilio Domicilio { get; set; }
    }
}
