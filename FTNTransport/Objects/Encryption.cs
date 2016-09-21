
using System.Linq;
using System.Security.Cryptography;
using System.Text;
namespace MyEncryption
{
    public class Encryption

    {
        public static string encrypt(string data)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(data));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }
    }
}