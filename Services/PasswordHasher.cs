using System.Collections;
using System.Security.Cryptography;

namespace ToDoList.Services
{
    public class PasswordHasher
    {
        private const int Iterations = 10000;

        private const int SaltSize = 16;

        private const int HashSize = 32;

        public (byte[] hash, byte[] salt) HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);
                    return (hash, salt);
                }
            }
        }

        public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hashToCheck = pbkdf2.GetBytes(HashSize);

                return StructuralComparisons.StructuralEqualityComparer.Equals(storedHash, hashToCheck);
            }
        }
    }
}
