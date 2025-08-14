namespace Huerto_Urbano_Backend.Interfaces
{
    public interface IServicioToken
    {
        string GenerateToken(int id, string nombre, string rol);
    }

}
