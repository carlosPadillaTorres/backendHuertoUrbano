using Huerto_Urbano_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Dto
{
        public class ProductoDto
        {
        
            [Required, StringLength(100)]
            public required string NombreProducto { get; set; }

            [Required, StringLength(30)]
            public required string Marca { get; set; }

            [Required, StringLength(3)]
            public required string Tipo { get; set; } // KIT, ACT, SEN, OTR, PLA.
            public decimal CostoUnidad { get; set; }

            public required string Descripcion { get; set; }

            public int IdProveedor { get; set; }

            [ForeignKey("IdProveedor")]
            public Proveedor? Proveedor { get; set; }
        }
    
}
