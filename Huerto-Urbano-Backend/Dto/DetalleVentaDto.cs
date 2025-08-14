namespace Huerto_Urbano_Backend.Dto
{
    public class DetalleVentaDto
    {
        public int IdDetalleVenta { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
