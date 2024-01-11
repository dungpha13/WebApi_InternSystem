using System.Security.Cryptography;
using System.Text;

namespace AmazingTech.InternSystem.Security
{
    public class ShieldGuardFtSHA256
    {
        public static string EncodeToken(string token)
        {
            SHA256 sha256 = SHA256.Create();

            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));

            return new Guid(hashBytes.Take(16).ToArray()).ToString();
        }
    }
}
