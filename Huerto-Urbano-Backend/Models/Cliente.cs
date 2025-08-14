using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Huerto_Urbano_Backend.Models
{
    public class Cliente
    {
        [Key] 
        public int IdCliente { get; set; }

        public int IdPersona { get; set; }

        [ForeignKey("IdPersona")]
        public required Persona Persona { get; set; }

        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public required Usuario Usuario { get; set; }
    }



}
