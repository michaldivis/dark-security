using DarkSecurity;
using DarkSecurity.Abstractions;

namespace SampleConsoleApp;

internal static class HashersDemo
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("-------------");
        Console.WriteLine("HASHERS demo");
        Console.WriteLine("-------------");

        // create an instance of a hasher
        IHasher hasher = new Pbkdf2Hasher();

        // hash password
        var passwordText = "Sup3rSaF3Passw0rd";
        var passwordHash = hasher.HashPassword(passwordText);
        Console.WriteLine($"Password text: {passwordText}");
        Console.WriteLine($"Password hash: {passwordHash}");

        // compare a plain text password to the hashed one
        var passwordMatchesHash = hasher.ComparePasswordToHash(passwordText, passwordHash); // returns true
        Console.WriteLine($"The same password matches: {passwordMatchesHash}");

        // compare a different plain text password to the hashed one
        var anotherPasswordText = "AnotherPasswordText";
        var anotherPasswordMatchesHash = hasher.ComparePasswordToHash(anotherPasswordText, passwordHash); // returns false
        Console.WriteLine($"A different password matches: {anotherPasswordMatchesHash}");
    }
}
