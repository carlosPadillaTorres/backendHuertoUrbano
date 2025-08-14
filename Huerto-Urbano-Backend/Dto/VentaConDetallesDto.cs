namespace Huerto_Urbano_Backend.Dto
{
    public class VentaConDetallesDto
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Total { get; set; }
        public bool Estatus { get; set; }
        public List<DetalleVentaDto> Detalles { get; set; } = new List<DetalleVentaDto>();
    }
}
