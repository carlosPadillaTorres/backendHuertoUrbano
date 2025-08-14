using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{
    public class LoteProducto
    {
        [Key]
        public int IdLote { get; set; }

        public int IdProducto { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        public DateTime FechaIngreso { get; set; }

        public int CantidadLote { get; set; }

        public decimal CostoTotal { get; set; }

        public int Existencia { get; set; }
    }

}
