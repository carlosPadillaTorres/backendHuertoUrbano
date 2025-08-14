using Huerto_Urbano_Backend.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Huerto_Urbano_Backend.Models
{
    public class Producto
    {
        [SetsRequiredMembers]
        public Producto() { }

        /*[SetsRequiredMembers]
        public Producto(ProductoDto producto)
        {
            IdProducto = 0;
            this.NombreProducto = producto.NombreProducto;
            Marca = producto.Marca;
            Tipo = producto.Tipo;
            CantidadTotal = 0;
            CostoUnidad = producto.CostoUnidad;
            Descripcion = producto.Descripcion;
            IdProveedor = producto.IdProveedor;
            Estatus = true;
        }*/

        public Producto InicializarProducto(ProductoDto producto)
        {
            return new Producto {
                IdProducto = 0,
                NombreProducto = producto.NombreProducto,
                Marca = producto.Marca,
                Tipo = producto.Tipo,
                CantidadTotal = 0,
                CostoUnidad = producto.CostoUnidad,
                Descripcion = producto.Descripcion,
                IdProveedor = producto.IdProveedor,
                Estatus = true
            };
        }

        [Key]
        public int IdProducto { get; set; }

        [Required, StringLength(100, ErrorMessage = "El nombre del producto no puede tener más de 100 caracteres")]
        public  required string NombreProducto { get; set; }

        [Required, StringLength(30, ErrorMessage ="La marca no puede tener más de 30 caracteres")]
        public required string Marca { get; set; }

        [Required, StringLength(3)]
        public string Tipo { get; set; } // KIT, ACT, SEN, OTR, PLA.

        public int CantidadTotal { get; set; }

        public decimal CostoUnidad { get; set; }

        public required string Descripcion { get; set; }

        public required int IdProveedor { get; set; }

        [ForeignKey("IdProveedor")]
        public  Proveedor? Proveedor { get; set; }

        public required bool Estatus { get; set; } = true;

        

    }

}
