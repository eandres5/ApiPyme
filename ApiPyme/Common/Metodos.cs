using System.Text;

namespace ApiPyme.Common
{
    public static class Metodos
    {
        public static string Key = "@l.a23snnasd-.";

        public static string Encrypt(string password) {
            if (string.IsNullOrEmpty(password)) return "";
            password += Key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        public static string Decrypt(string base64Encode)
        {
            if (string.IsNullOrEmpty(base64Encode)) return "";
            var baseEncodeBytes = Convert.FromBase64String(base64Encode);
            var result = Encoding.UTF8.GetString(baseEncodeBytes);
            result = result.Substring(0, result.Length - Key.Length);
            return result;
        }
    }
}
