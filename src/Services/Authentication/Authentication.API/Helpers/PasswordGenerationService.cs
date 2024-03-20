using Authentication.API.Helpers.Common;
using System.Security.Cryptography;
using System.Text;

namespace Authentication.API.Helpers
{
    public class PasswordGenerationService : IPasswordGenerationService
    {
        public bool VerifyPassword(string storedSalt, string storedHashedPassword, string inputPassword)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);
            byte[] combinedBytes = Combine(Encoding.UTF8.GetBytes(inputPassword), salt);
            byte[] hashedBytes = HashBytes(combinedBytes);
            string computedHash = Convert.ToBase64String(hashedBytes);

            return storedHashedPassword == computedHash;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }
            return salt;
        }

        public byte[] Combine(byte[] passwordBytes, byte[] salt)
        {
            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);
            return combinedBytes;
        }

        public byte[] HashBytes(byte[] inputBytes)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(inputBytes);
            }
        }
    }
}
