using System.ComponentModel.DataAnnotations;

namespace Huerto_Urbano_Backend.Models
{
    public class Servicio
    {
        [Key]
        public int IdServicio { get; set; }

        [Required, StringLength(100)]
        public string NombreServicio { get; set; }

        public string Descripcion { get; set; }

        public decimal PrecioBase { get; set; }
    }

}
