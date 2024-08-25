using System.Security.Cryptography;
using System.Text;

namespace AltenBlog.Common.Infrastructore
{
    public class PasswordEncryptor
    {
        public static string Encrpt(string password)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);//string burada bytearraye cevriliyor
            byte[] hashBytes = md5.ComputeHash(inputBytes);//hash lenmis olan bytlara ceviriyor, burad sfrelenmis data oluyor

            return Convert.ToHexString(hashBytes);// sfrelenmis datayi stringe ceririp döndürüyor
        }
    }
}
