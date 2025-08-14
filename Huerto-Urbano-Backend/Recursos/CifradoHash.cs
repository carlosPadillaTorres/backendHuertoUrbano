namespace Huerto_Urbano_Backend.Recursos
{
    public class CifradoHash
    {
        public static string Cifrar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                throw new ArgumentException("El texto no puede ser nulo o vacío.", nameof(texto));
            }
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(texto);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

    }
}
