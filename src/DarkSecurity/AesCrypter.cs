using DarkSecurity.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DarkSecurity
{
    public class AesCrypter : IDisposable, ICrypter
    {
        private readonly Aes _encryptor;
        private readonly ILogger<AesCrypter> _logger;

        public AesCrypter(string key, byte[] IV, ILogger<AesCrypter> logger)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException(nameof(IV));
            }

            _logger = logger;

            _encryptor = Aes.Create();

            var pdb = new Rfc2898DeriveBytes(key, IV);

            _encryptor.Key = pdb.GetBytes(16);
            _encryptor.IV = pdb.GetBytes(16);
        }

        public AesCrypter(string key, byte[] IV) : this(key, IV, NullLoggerFactory.Instance.CreateLogger<AesCrypter>()) { }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }

            var clearBytes = Encoding.Unicode.GetBytes(plainText);

            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, _encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                    }

                    plainText = Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't encrypt the text");
                throw new CryptException("Couldn't encrypt the text", ex);
            }

            return plainText;
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return cipherText;
            }

            try
            {
                cipherText = cipherText.Replace(" ", "+");
                var cipherBytes = Convert.FromBase64String(cipherText);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, _encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't decrypt the text");
                throw new CryptException("Couldn't decrypt the text", ex);
            }

            return cipherText;
        }

        public async Task<string> EncryptAsync(string text)
        {
            return await Task.Run(() => Encrypt(text));
        }

        public async Task<string> DecryptAsync(string text)
        {
            return await Task.Run(() => Decrypt(text));
        }

        public void Dispose()
        {
            _encryptor.Dispose();
        }
    }
}
