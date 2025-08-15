using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{
    public class ComprasRealizadas
    {
        [Key] 
        public int IdComprasRealizadas { get; set; }

        public int IdProveedor { get; set; }

        [ForeignKey("IdProveedor")]
        public Proveedor Proveedor { get; set; }

        public decimal Precio { get; set; }

        public DateTime Fecha { get; set; }

        [Required, StringLength(11)]
        public string NumeroOrden { get; set; }

        public bool Estatus { get; set; } = false;

        public int? idEmpleado { get; set; }
    }

}
