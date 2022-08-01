using DarkSecurity;
using DarkSecurity.Abstractions;

namespace SampleConsoleApp;
internal static class CryptersDemo
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("-------------");
        Console.WriteLine("CRYPTERS demo");
        Console.WriteLine("-------------");

        // create an instance of a crypter
        var key = "someKey";
        var IV = new byte[] { 1, 3, 5, 9, 1, 4, 5, 6 };
        ICrypter crypter = new AesCrypter(key, IV);

        Console.WriteLine($"Key: {key}");
        Console.WriteLine($"IV: {IV}");

        // encrypt text
        var plainText = "A happy little tree";
        var cipherText = crypter.Encrypt(plainText); // produces "GeUvzJXcLMR0uwieqQuJmEX48LcC+5anQNxRrQgPG5n3rDjhw/8sPkiTV3KacUwV"
        Console.WriteLine($"Plain text: {plainText}");
        Console.WriteLine($"Encrypted text: {cipherText}");

        // decrypt text
        var decryptedText = crypter.Decrypt(cipherText); // produces "A happy little tree"
        Console.WriteLine($"Decrypted text: {decryptedText}");
    }
}
