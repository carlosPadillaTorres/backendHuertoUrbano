using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{
    public class Ciudad
    {
        [Key] 
        public int IdCiudad { get; set; }

        [Required]
        [StringLength(30)]
        public required string nombreCiudad { get; set; }

        public int IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public Estado Estado { get; set; }

        //public ICollection<Domicilio> Domicilios { get; set; }
    }

}
