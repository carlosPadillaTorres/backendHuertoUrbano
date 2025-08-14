using Huerto_Urbano_Backend.Dto;
using System.Security.Claims;

namespace Huerto_Urbano_Backend.Recursos
{
    public class DecodificadorToken
    {

        public static UsuarioLogeadoDto DecodificarToken(string token)
        {
            try
            {

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("No se encontró el token en la solicitud");
                    return null;
                }

                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

                if (!handler.CanReadToken(token))
                {
                    Console.WriteLine("Formato de token inválido");
                    return null;
                }

                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToList();
                return new UsuarioLogeadoDto
                (
                    int.Parse(claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value),
                    claims.FirstOrDefault(c => c.Type == "nombreUsuario")?.Value,
                    claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                );
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al decodificar el token: " + e.Message);
                return null;
            }
        }

    }
}
