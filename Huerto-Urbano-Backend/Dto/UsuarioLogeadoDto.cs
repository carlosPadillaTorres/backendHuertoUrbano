namespace Huerto_Urbano_Backend.Dto
{
    public class UsuarioLogeadoDto
    {
        public UsuarioLogeadoDto(int idUsuario, string nombreUsuario, string rol)
        {
            this.idUsuario = idUsuario;
            this.nombreUsuario = nombreUsuario;
            this.rol = rol;
        }

        public int idUsuario {  get; set; }
        public string nombreUsuario { get; set; }
        public string rol { get; set; }
    }
}
