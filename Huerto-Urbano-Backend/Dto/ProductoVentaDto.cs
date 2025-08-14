using System.ComponentModel.DataAnnotations;

namespace Huerto_Urbano_Backend.Dto
{
    public class ProductoVentaDto
    {
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
}
