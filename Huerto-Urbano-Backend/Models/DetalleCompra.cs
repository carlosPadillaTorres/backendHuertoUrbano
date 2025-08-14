using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{
    public class DetalleCompra
    {
        [Key] 
        public int IdDetalleCompra { get; set; }

        public int IdComprasRealizadas { get; set; }

        [ForeignKey("IdComprasRealizadas")]
        public ComprasRealizadas ComprasRealizadas { get; set; }

        public int IdProducto { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }

}
