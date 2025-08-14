using Huerto_Urbano_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Dto
{

    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public  PersonaDto Persona { get; set; }

        public static Cliente CrearClienteActualizacion(ClienteDto datosCliente, Cliente cliente)
        {
            cliente.Persona.ApPaterno = datosCliente.Persona.ApPaterno;
            cliente.Persona.ApMaterno = datosCliente.Persona.ApMaterno;
            cliente.Persona.Nombre = datosCliente.Persona.Nombre;
            cliente.Persona.FechaNacimiento = datosCliente.Persona.FechaNacimiento;
            cliente.Persona.Email = datosCliente.Persona.Email;
            cliente.Persona.Telefono = datosCliente.Persona.Telefono;
            cliente.Persona.Genero = datosCliente.Persona.Genero;
            cliente.Persona.IdDomicilio = datosCliente.Persona.IdDomicilio;

            cliente.Persona.Domicilio.IdDomicilio = datosCliente.Persona.Domicilio.IdDomicilio;
            cliente.Persona.Domicilio.Calle = datosCliente.Persona.Domicilio.Calle;
            cliente.Persona.Domicilio.Numero = datosCliente.Persona.Domicilio.Numero;
            cliente.Persona.Domicilio.Colonia = datosCliente.Persona.Domicilio.Colonia;
            cliente.Persona.Domicilio.CodigoPostal = datosCliente.Persona.Domicilio.CodigoPostal;
            cliente.Persona.Domicilio.IdCiudad = datosCliente.Persona.Domicilio.IdCiudad;

            return cliente;
        }
    }


      



}

