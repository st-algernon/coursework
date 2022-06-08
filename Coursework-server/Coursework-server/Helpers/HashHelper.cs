using System.Security.Cryptography;
using System.Text;
using Coursework_server.Configs;
using SHA3.Net;

namespace Coursework_server.Helpers
{
    public class HashHelper
    {
        public static string ComputeSha3Hash(string data)
        {
            using var sha3512 = Sha3.Sha3512();
            using var sha256Hash = SHA256.Create();
            var bytes = sha3512.ComputeHash(Encoding.UTF8.GetBytes(data + HashConfig.Salt));

            return ConvertToHex(bytes);
        }

        public static string ConvertToHex(byte[] bytes)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
