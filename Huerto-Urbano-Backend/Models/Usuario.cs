using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Huerto_Urbano_Backend.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required, StringLength(20)]
        public required string NombreUsuario { get; set; }
        [DefaultValue("")]
        public string? Token { get; set; } 

        public bool Estatus { get; set; } = true;

        [Required]
        public required string Contrasenia { get; set; }

        [Required, StringLength(4)]
        public required string Rol { get; set; } // ADMS, EMPL, CLIE


        public static Usuario OcultarInfoSensible(Usuario usuario) {
            return new Usuario { 
                Contrasenia="",
                IdUsuario=0,
                Rol="",
                Estatus=false,
                Token="",
                NombreUsuario = usuario.NombreUsuario
            };
        }
            
        

    }
    


}
