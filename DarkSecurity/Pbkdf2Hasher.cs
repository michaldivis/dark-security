using DarkSecurity.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace DarkSecurity
{
    public class Pbkdf2Hasher : IHasher
    {
        private readonly ILogger<Pbkdf2Hasher> _logger;

        public Pbkdf2Hasher(ILogger<Pbkdf2Hasher> logger)
        {
            _logger = logger;
        }

        public Pbkdf2Hasher() : this(NullLoggerFactory.Instance.CreateLogger<Pbkdf2Hasher>()) { }

        public string HashPassword(string passwordText)
        {
            if (passwordText is null)
            {
                throw new ArgumentNullException(nameof(passwordText));
            }

            try
            {
                // 1.-Create the salt value with a cryptographic PRNG
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[20]);

                // 2.-Create the RFC2898DeriveBytes and get the hash value
                var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, 100000);
                var hash = pbkdf2.GetBytes(20);

                // 3.-Combine the salt and password bytes for later use
                var hashBytes = new byte[40];
                Array.Copy(salt, 0, hashBytes, 0, 20);
                Array.Copy(hash, 0, hashBytes, 20, 20);

                // 4.-Turn the combined salt+hash into a string for storage
                var hashPass = Convert.ToBase64String(hashBytes);

                return hashPass;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create password hash from text");
                throw new HashException("Failed to create password hash from text", ex);
            }
        }

        public async Task<string> HashPasswordAsync(string passwordText)
        {
            return await Task.Run(() => HashPassword(passwordText));
        }

        public bool ComparePasswordToHash(string passwordText, string passwordHash)
        {
            if (passwordText is null)
            {
                throw new ArgumentNullException(nameof(passwordText));
            }

            if (passwordHash is null)
            {
                throw new ArgumentNullException(nameof(passwordHash));
            }

            try
            {
                // Extract the bytes
                byte[] hashBytes = Convert.FromBase64String(passwordHash);
                // Get the salt
                byte[] salt = new byte[20];
                Array.Copy(hashBytes, 0, salt, 0, 20);
                // Compute the hash on the password the user entered
                var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);
                // compare the results
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 20] != hash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to compare passwordText to passwordHash");
                throw new HashException("Failed to compare passwordText to passwordHash", ex);
            }
        }

        public async Task<bool> ComparePasswordToHashAsync(string passwordText, string passwordHash)
        {
            return await Task.Run(() => ComparePasswordToHash(passwordText, passwordHash));
        }
    }
}
