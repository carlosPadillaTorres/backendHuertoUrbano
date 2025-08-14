using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{
    public class Asesoria
    {
        [Key]
        public int IdAsesoria { get; set; }

        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        public int IdServicio { get; set; }

        [ForeignKey("IdServicio")]
        public Servicio Servicio { get; set; }

        public int? IdEmpleado { get; set; }

        [ForeignKey("IdEmpleado")]
        public Empleado Empleado { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        public DateTime? FechaProgramada { get; set; }

        public DateTime? FechaRealizacion { get; set; }

        public int Estatus { get; set; } = 0;

        public string Observaciones { get; set; }

        public decimal? CostoTotal { get; set; }
    }

}
