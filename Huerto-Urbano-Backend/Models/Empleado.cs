using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Recursos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Huerto_Urbano_Backend.Models
{
    public class Empleado
    {
        [SetsRequiredMembers]
        public Empleado()
        {
        }

        internal Empleado(EmpleadoDto emp)
        {

            Puesto = emp.Puesto;
            Curp = emp.Curp;
            Rfc = emp.Rfc;
            SalarioBruto = emp.SalarioBruto;
            FechaIngreso = emp.FechaIngreso;
            Persona = PersonaDto.InicializarPersona(emp.Persona);
            Usuario = emp.Usuario;
            Usuario.Contrasenia = CifradoHash.Cifrar(emp.Usuario.Contrasenia);
        }

        [Key] 
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(45, ErrorMessage="El puesto no puede tener más de 45 caracteres.")]
        public string Puesto { get; set; }

        [Required, StringLength(18, ErrorMessage = "El CURP no puede tener más de 18 caracteres.")]
        public string Curp { get; set; }

        [Required, StringLength(13, ErrorMessage = "El RFC no puede tener más de 13 caracteres.")]
        public string Rfc { get; set; }

        [Required]
        public double SalarioBruto { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaRenuncia { get; set; }

        public int IdPersona { get; set; }

        [ForeignKey("IdPersona")]
        public Persona Persona { get; set; }

        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
    }



}
