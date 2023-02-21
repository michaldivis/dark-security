using DarkSecurity.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace DarkSecurity
{
    public class Pbkdf2Hasher : IHasher
    {
        private const int _iterations = 100_000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA1;

        private readonly ILogger<Pbkdf2Hasher> _logger;

        public Pbkdf2Hasher(ILogger<Pbkdf2Hasher> logger)
        {
            _logger = logger;
        }

        public Pbkdf2Hasher() : this(NullLoggerFactory.Instance.CreateLogger<Pbkdf2Hasher>()) 
        { 
        }

        public string HashPassword(string passwordText)
        {
            if (passwordText is null)
            {
                throw new ArgumentNullException(nameof(passwordText));
            }

            try
            {
                // Create the random salt value
#if NET6_0_OR_GREATER
                byte[] salt = RandomNumberGenerator.GetBytes(20);
#else
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[20]);
#endif

                // Create the RFC2898DeriveBytes and get the hash value
                var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, _iterations, _hashAlgorithmName);

                var hash = pbkdf2.GetBytes(20);

                // Combine the salt and password bytes for later use
                var hashBytes = new byte[40];
                Array.Copy(salt, 0, hashBytes, 0, 20);
                Array.Copy(hash, 0, hashBytes, 20, 20);

                // Turn the combined salt+hash into a string for storage
                var hashText = Convert.ToBase64String(hashBytes);

                return hashText;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create password hash from text");
                throw new HashException("Failed to create password hash from text", ex);
            }
        }

        public async Task<string> HashPasswordAsync(string passwordText, CancellationToken ct = default)
        {
            return await Task.Run(() => HashPassword(passwordText), ct).ConfigureAwait(false);
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
                var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, _iterations, _hashAlgorithmName);
                byte[] hash = pbkdf2.GetBytes(20);

                // Compare the results
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

        public async Task<bool> ComparePasswordToHashAsync(string passwordText, string passwordHash, CancellationToken ct = default)
        {
            return await Task.Run(() => ComparePasswordToHash(passwordText, passwordHash), ct).ConfigureAwait(false);
        }
    }
}
