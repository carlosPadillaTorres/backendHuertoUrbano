using System.ComponentModel.DataAnnotations;

namespace Huerto_Urbano_Backend.Dto
{
    public class RegistrarVentaDto
    {
        [Required]
        public decimal Total { get; set; }
        [Required]
        public List<ProductoVentaDto> Productos { get; set; } = new List<ProductoVentaDto>();
    }

    
}

