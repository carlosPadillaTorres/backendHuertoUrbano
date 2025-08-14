using Huerto_Urbano_Backend.Models;

namespace Huerto_Urbano_Backend.Dto
{
    public class DomicilioDto
    {
        public int IdDomicilio { get; set; }

        public string Calle { get; set; }

        public string Numero { get; set; }

        public string Colonia { get; set; }

        public string CodigoPostal { get; set; }

        public int IdCiudad { get; set; }
    

        public static Domicilio InicializarDomicilio(DomicilioDto dom)
        {  // Devuelve un objeto tipo Domiclio apartir de objeto DomicilioDto
            return new Domicilio
            {
                IdDomicilio = dom.IdDomicilio,
                Calle= dom.Calle,
                CodigoPostal = dom.CodigoPostal,
                Colonia=dom.Colonia,
                Numero = dom.Numero,
                IdCiudad = dom.IdCiudad

            };
        }
    }
}
