using System.Security.Cryptography;
using System.Text;

namespace Wallet_grupo1.Helpers
{
    public class PasswordEncryptHelper
    {
        public static string  EncryptPassword(string str)
        {
            SHA256 sha256 = SHA256.Create();    
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = Array.Empty<byte>();    
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:2}", stream[i]);
            }
            return sb.ToString();
        }
    }
}
