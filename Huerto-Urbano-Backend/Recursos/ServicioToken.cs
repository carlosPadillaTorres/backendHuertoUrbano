using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Huerto_Urbano_Backend.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Huerto_Urbano_Backend.Recursos
{

    public class ServicioToken
    {
        private readonly IConfiguration _config;

        public ServicioToken(IConfiguration config)
        {
            _config = config;
        }

            public static string GenerateToken(int id, string nombre, string rol)
            {
                var jwt = new
                {
                    Key = "ClaveSuperSecreta12345678900485SS",
                    Issuer = "https://tuapi.com",
                    Audience = "https://tuapp.com",
                    ExpireMinutes = 60
                };


                var claims = new[]
            {
                new Claim("idUsuario", id.ToString()),
                new Claim("nombreUsuario", nombre),
                new Claim("rol", rol),
                new Claim(ClaimTypes.Role, rol)

            };
                try
                {
                   // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)); 
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var token = new JwtSecurityToken(
                    issuer: jwt.Issuer,
                    audience: jwt.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes((jwt.ExpireMinutes)),
                    signingCredentials: creds
                );
            
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR al generar el token: " + e.Message);
                    throw new Exception("Error al generar el token");
                }
            }



      

    }

}
