using DarkSecurity;
using DarkSecurity.Abstractions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DarkSecurityTests
{
    public class AesCrypterTests
    {
        private readonly ICrypter _sut;

        public static IEnumerable<object[]> TestData => _testData.Select(a => new object[] { a.plainText, a.cipherText });

        private static IEnumerable<(string plainText, string cipherText)> _testData = new[]
        {
            ("some text", "qzV6b5LNgxmfu+GxxPkp9u0zLvhYqNBzsTmz/FMZfPM="),
            ("monday forecast", "dBWQgeNTz8PoPFkUd6QMoXNk1N+MWn/ZEh2VCc0+eBE="),
            ("1", "7UIs0wu1iIcSN/CWdesFqg=="),
            ("a wonderful pony went to the candy valley", "dBvNuLulYeYIMI/k86pPhW1/uQNt0HFqZGE1RjdixIstxfdR/HdFxYG7TiUP80R5k0bCEr4mNF0G4F16lsjQaLGwNul8P2O9CSFucVxlclHoAANPItlAump6TfRQu5ww")
        };

        public AesCrypterTests()
        {
            _sut = new AesCrypter("someKey", new byte[] { 1, 3, 5, 9, 1, 4, 5, 6 });
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(null, new byte[] { })]
        [InlineData("", new byte[] { })]
        public void Ctor_ShouldThrow_WhenArgumentsInvalid(string key, byte[] IV)
        {
            Action act = () => new AesCrypter(key, IV);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void EncryptAndDecrypt_ShouldResultEqualTheOriginalText_WhenTextIsValid(string plainText, string cipherText)
        {
            var encrypted = _sut.Encrypt(plainText);
            var decrypted = _sut.Decrypt(encrypted);

            decrypted.Should().Be(plainText);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        public void Encrypt_ShouldProduceEmptyText_WhenTextIsInvalid(string plainText, string expected)
        {
            var result = _sut.Encrypt(plainText);

            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Encrypt_ShouldProduceCipherText_WhenTextIsValid(string plainText, string cipherText)
        {
            var result = _sut.Encrypt(plainText);

            result.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        public void Decrypt_ShouldProduceEmptyText_WhenCipherTextIsInvalid(string cipherText, string expected)
        {
            var result = _sut.Decrypt(cipherText);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("invalid cipher text")]
        public void Decrypt_ShouldThrow_WhenCipherTextIsInvalid(string cipherText)
        {
            Action act = () => _sut.Decrypt(cipherText);

            act.Should().Throw<CryptException>();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Decrypt_ShouldProducePlainText_WhenCipherTextIsValid(string expected, string cipherText)
        {
            var result = _sut.Decrypt(cipherText);

            result.Should().Be(expected);
        }

        #region Async methods

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        public async Task EncryptAsync_ShouldProduceEmptyText_WhenTextIsInvalid(string plainText, string expected)
        {
            var result = await _sut.EncryptAsync(plainText);

            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task EncryptAsync_ShouldProduceCipherText_WhenTextIsValid(string plainText, string cipherText)
        {
            var result = await _sut.EncryptAsync(plainText);

            result.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        public async Task DecryptAsync_ShouldProduceEmptyText_WhenCipherTextIsInvalid(string cipherText, string expected)
        {
            var result = await _sut.DecryptAsync(cipherText);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("invalid cipher text")]
        public async Task DecryptAsync_ShouldThrow_WhenCipherTextIsInvalid(string cipherText)
        {
            Func<Task> act = () => _sut.DecryptAsync(cipherText);

            await act.Should().ThrowAsync<CryptException>();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task DecryptAsync_ShouldProducePlainText_WhenCipherTextIsValid(string expected, string cipherText)
        {
            var result = await _sut.DecryptAsync(cipherText);

            result.Should().Be(expected);
        }

        #endregion
    }
}
