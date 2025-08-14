using System;
using System.Collections.Generic;

namespace Huerto_Urbano_Backend.Dto
{
    public class RegistrarCompraDto
    {
        public int ProveedorId { get; set; }
        public DateTime FechaCompra { get; set; }
        public List<DetalleCompraDto> Detalles { get; set; }
    }

    public class DetalleCompraDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
