using Huerto_Urbano_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Dto
{
    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }
        [Required]
        [StringLength(45, ErrorMessage = "El puesto no puede tener más de 45 caracteres.")]
        public string Puesto { get; set; }

        [Required, StringLength(18, ErrorMessage = "El CURP no puede tener más de 18 caracteres.")]
        public string Curp { get; set; }

        [Required, StringLength(13, ErrorMessage = "El RFC no puede tener más de 13 caracteres.")]
        public string Rfc { get; set; }
        public double SalarioBruto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public PersonaDto Persona { get; set; }
        public Usuario  Usuario { get; set; }
    }

    /*public class ContactoEmpleadoDto
    {
        public ContactoEmpleadoDto(Empleado emp)
        {
            IdEmpleado = emp.IdEmpleado;

            Persona.ApPaterno = emp.Persona.ApPaterno;
            Persona.ApMaterno = emp.Persona.ApMaterno;
            Persona.Nombre = emp.Persona.Nombre;
            Persona.Telefono = emp.Persona.Telefono;
            Persona.Genero = emp.Persona.Genero;
            Persona.FechaNacimiento = emp.Persona.FechaNacimiento;
            Persona.Email = emp.Persona.Email;

            Persona.Domicilio.Calle = emp.Persona.Domicilio.Calle;
            Persona.Domicilio.Colonia = emp.Persona.Domicilio.Colonia;
            Persona.Domicilio.CodigoPostal = emp.Persona.Domicilio.CodigoPostal;
            Persona.Domicilio.Numero = emp.Persona.Domicilio.Numero;
            Persona.Domicilio.IdDomicilio = emp.Persona.Domicilio.IdDomicilio;
        }

        public int IdEmpleado { get; set; }
        public   Persona { get; set; }
    }*/
}
