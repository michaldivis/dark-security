using System.Threading.Tasks;
using System;

namespace DarkSecurity.Abstractions
{
    /// <summary>
    /// Converts a password in plain text into a hash (one way only conversion)
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// Creates a hash for a password in plain text
        /// </summary>
        /// <param name="passwordText">Password in plain text</param>
        /// <returns>Password hash</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="HashException" />
        string HashPassword(string passwordText);

        /// <summary>
        /// Creates a hash for a password in plain text asynchronously
        /// </summary>
        /// <param name="passwordText">Password in plain text</param>
        /// <returns>Password hash</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="HashException" />
        Task<string> HashPasswordAsync(string passwordText);

        /// <summary>
        /// Checks if a password in plain text is the same as password stored in a hash
        /// </summary>
        /// <param name="passwordText">Password in plain text</param>
        /// <param name="passwordHash">Password hash to compare</param>
        /// <returns><see langword="true"/> if the passwords match</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="HashException" />
        bool ComparePasswordToHash(string passwordText, string passwordHash);

        /// <summary>
        /// Checks if a password in plain text is the same as password stored in a hash asynchronously
        /// </summary>
        /// <param name="passwordText">Password in plain text</param>
        /// <param name="passwordHash">Password hash to compare</param>
        /// <returns><see langword="true"/> if the passwords match</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="HashException" />
        Task<bool> ComparePasswordToHashAsync(string passwordText, string passwordHash);
    }
}