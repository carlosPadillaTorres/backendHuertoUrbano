using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{
    public class Domicilio
    {
        [Key] 
        public int IdDomicilio { get; set; }

        [Required]
        [StringLength(100)]
        public string Calle { get; set; }

        [Required]
        [StringLength(5)]
        public string Numero { get; set; }

        [Required]
        [StringLength(100)]
        public string Colonia { get; set; }

        [Required]
        [StringLength(5)]
        public string CodigoPostal { get; set; }

        public int IdCiudad { get; set; }

        [ForeignKey("IdCiudad")]
        public Ciudad? Ciudad { get; set; }
    }

}
