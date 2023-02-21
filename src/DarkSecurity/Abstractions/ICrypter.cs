using System.Threading.Tasks;
using System;
using System.Threading;

namespace DarkSecurity.Abstractions
{
    /// <summary>
    /// Converts plain text into an encrypted cipher text (two way conversion)
    /// </summary>
    public interface ICrypter
    {
        /// <summary>
        /// Encrypts plain text into cipher text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Cipher text</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="CryptException" />
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts cipher text into plain text
        /// </summary>
        /// <param name="encryptedText">Cipher text to decrypt</param>
        /// <returns>Plain text</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="CryptException" />
        string Decrypt(string encryptedText);

        /// <summary>
        /// Encrypts plain text into cipher text asynchronously
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Cipher text</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="CryptException" />
        Task<string> EncryptAsync(string plainText, CancellationToken ct = default);

        /// <summary>
        /// Decrypts cipher text into plain text asynchronously
        /// </summary>
        /// <param name="encryptedText">Cipher text to decrypt</param>
        /// <returns>Plain text</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="CryptException" />
        Task<string> DecryptAsync(string encryptedText, CancellationToken ct = default);
    }
}
