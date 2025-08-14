using Huerto_Urbano_Backend.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Huerto_Urbano_Backend.Models
{
    public class Proveedor
    {
        [SetsRequiredMembers]
        public Proveedor() { 
        }
        [Key]
        public int IdProveedor { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Required(ErrorMessage = "El nombre de empresa es obligatorio.")]
        public string Empresa { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        public DateTime? FechaTermino { get; set; }

        public bool Estatus { get; set; } = true;

        [Required]
        [StringLength(10, ErrorMessage = "El teléfono no puede tener más de 10 caracteres.")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El correo no no tiene el formato correcto.")]
        [StringLength(100, ErrorMessage = "El correo no puede tener más de 100 caracteres.")]
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "El RFC no puede tener más de 12 caracteres.")]
        public string Rfc { get; set; }

        public int IdDomicilio { get; set; }

        [ForeignKey("IdDomicilio")]
        public Domicilio Domicilio { get; set; }

        internal Proveedor(ProveedorDto proveedor)
        {
            Empresa = proveedor.Empresa;
            FechaRegistro = DateTime.Now;
            Telefono = proveedor.Telefono;
            Estatus = true; // Por defecto, el proveedor está activo al ser creado.
            Email = proveedor.Email;
            Rfc = proveedor.Rfc;
            Domicilio = proveedor.Domicilio;
        }
    }

}
