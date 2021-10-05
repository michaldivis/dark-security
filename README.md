# DarkSecurity
Encryption and hashing tools

<!-- Status: -->

<!-- ![Unit tests](https://github.com/michaldivis/dark-security/workflows/unit_tests/badge.svg) -->

## Nuget
[![Nuget](https://img.shields.io/nuget/v/divis.darksecurity?label=DarkSecurity)](https://www.nuget.org/packages/Divis.DarkSecurity/)

## Features

### Crypters

```csharp
// create an instance of a crypter
var key = "someKey";
var IV = new byte[] { 1, 3, 5, 9, 1, 4, 5, 6 };
ICrypter crypter = new AesCrypter(key, IV);

// encrypt text
var plainText = "A happy little tree";
var cipherText = crypter.Encrypt(plainText); // produces "GeUvzJXcLMR0uwieqQuJmEX48LcC+5anQNxRrQgPG5n3rDjhw/8sPkiTV3KacUwV"

// decrypt text
var decrypted = crypter.Decrypt(cipherText); // produces "A happy little tree"
```

The `Encrypt` and `Decrypt` methods also have async versions

```csharp
// encrypt text async
var plainText = "A happy little tree";
var cipherText = await crypter.EncryptAsync(plainText); // produces "GeUvzJXcLMR0uwieqQuJmEX48LcC+5anQNxRrQgPG5n3rDjhw/8sPkiTV3KacUwV"

// decrypt text async
var decrypted = await crypter.DecryptAsync(cipherText); // produces "A happy little tree"
```

### Hashers

```csharp
// create an instance of a hasher
IHasher hasher = new Pbkdf2Hasher();

// hash password
var passwordText = "Sup3rSaF3Passw0rd";
var passwordHash = hasher.HashPassword(passwordText);

// compare a plain text password to the hashed one
var passwordMatchesHash = hasher.ComparePasswordToHash(passwordText, passwordHash); // returns true

// compare a different plain text password to the hashed one
var anotherPasswordText = "AnotherPasswordText";
var anotherPasswordMatchesHash = hasher.ComparePasswordToHash(anotherPasswordText, passwordHash); // returns false
```

The `HashPassword` and `ComparePasswordToHash` methods also have async versions
```csharp
// hash password async
var passwordText = "Sup3rSaF3Passw0rd";
var passwordHash = await hasher.HashPasswordAsync(passwordText);

// compare a plain text password to the hashed one async
var passwordMatchesHash = await hasher.ComparePasswordToHashAsync(passwordText, passwordHash); // returns true

// compare a different plain text password to the hashed one async
var anotherPasswordText = "AnotherPasswordText";
var anotherPasswordMatchesHash = await hasher.ComparePasswordToHashAsync(anotherPasswordText, passwordHash); // returns false
```

### `ILogger` support
All the crypters and hashers can also take an instance of `ILogger` in the constructor
```csharp
string key;
byte[] IV;
ILogger<AesCrypter> logger;

var crypter = new AesCrypter(key, IV, logger);
```

```csharp
ILogger<Pbkdf2Hasher> logger;

var hasher = new Pbkdf2Hasher(logger);
```