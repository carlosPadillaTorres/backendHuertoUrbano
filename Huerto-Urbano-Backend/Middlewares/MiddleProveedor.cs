using Huerto_Urbano_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.Entity.Validation;
using System.Text;
using System.Text.Json;

namespace Huerto_Urbano_Backend.Middlewares
{
    public class MiddleActualizarProveedor
    {
        private readonly RequestDelegate _next;

            public MiddleActualizarProveedor(RequestDelegate next)
            {
                _next = next;
            }
    
        public async Task InvokeAsync(HttpContext context)
        {
            // Permitir volver a leer el body
            context.Request.EnableBuffering();
            Proveedor proveedor ;
            try
            {
                // Leer el body
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true);

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                Console.WriteLine("Middle - Proveedor: " + body);

                var modelo = JsonSerializer.Deserialize<Proveedor>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                proveedor = modelo;
            }
            catch (DbEntityValidationException ex)
            {
                //return BadRequest(ex);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "ERROR de formato del cuerpo de la petición: "+ex
                });
            }
            await _next(context);
        }

    }
}
