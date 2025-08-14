using System.ComponentModel.DataAnnotations;

namespace Huerto_Urbano_Backend.Models
{
    public class    Estado
    {
        [Key]
        public int IdEstado { get; set; }

        [Required]
        [StringLength(30)]
        public string NombreEstado { get; set; }

        //public ICollection<Ciudad> Ciudades { get; set; }
    }

}
