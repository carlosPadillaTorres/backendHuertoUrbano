using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }

        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        public DateTime FechaVenta { get; set; } = DateTime.Now;

        public decimal Total { get; set; }

        public bool Estatus { get; set; } = false;

    }

}
