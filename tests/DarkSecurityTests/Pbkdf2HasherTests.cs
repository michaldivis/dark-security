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
    public class Pbkdf2HasherTests 
    {
        private readonly IHasher _sut;

        public static IEnumerable<object[]> TestData => _testData.Select(a => new object[] { a.plainText, a.hash });

        private static IEnumerable<(string plainText, string hash)> _testData = new[]
        {
            ("a wonderful pony went to the candy valley", "TwpSji1ePFJId9tcAykqz4CUHCAUvmRypslk3xSTns8TuJXJtHpd+g=="),
            ("monday forecast", "5FeQnl1ADZzfXasycPEHIYzZ/UY55v4ATtLxCJEVlQQ0xhPOQhyQyQ=="),
            ("some text", "TWT8qnOf6B+l8EzmNW42FeXWCJ3tpUNFEmAAN6uRX24rSsjU6oY98Q=="),
            ("1", "8J8xX8ba0VFCwcfWJQmeL8H/tROwyn8D1kv92At/jIWQKAdQxpiM/Q=="),
            ("", "H96FSN+ij3tgX7ZkEl5ipkIYMbOKjkYrCfHBsbPFdkc3bbTnsXLkmQ==")
        };

        public Pbkdf2HasherTests()
        {
            _sut = new Pbkdf2Hasher();
        }

        [Theory]
        [InlineData(null)]
        public void HashPassword_ShouldThrow_WhenTextIsInvalid(string text)
        {
            Action act = () => _sut.HashPassword(text);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void HashPassword_ShouldProduceValidHash_WhenTextIsValid(string passwordText, string passwordHash)
        {
            var result = _sut.HashPassword(passwordText);

            result.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("valid", null)]
        [InlineData(null, "valid")]
        public void ComparePasswordToHash_ShouldThrow_WhenArgumentsInvalid(string passwordText, string passwordHash)
        {
            Action act = () => _sut.ComparePasswordToHash(passwordText, passwordHash);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void ComparePasswordToHash_ShouldBeTrue_WhenPasswordsMatch(string passwordText, string passwordHash)
        {
            var result = _sut.ComparePasswordToHash(passwordText, passwordHash);

            result.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void ComparePasswordToHash_ShouldBeFalse_WhenPasswordsDontMatch(string passwordText, string passwordHash)
        {
            var result = _sut.ComparePasswordToHash("differentPasswordText", passwordHash);

            result.Should().BeFalse();
        }

        #region Async methods

        [Theory]
        [InlineData(null)]
        public async Task HashPasswordAsync_ShouldThrow_WhenTextIsInvalid(string text)
        {
            Func<Task> act = () => _sut.HashPasswordAsync(text);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task HashPasswordAsync_ShouldProduceValidHash_WhenTextIsValid(string passwordText, string passwordHash)
        {
            var result = await _sut.HashPasswordAsync(passwordText);

            result.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("valid", null)]
        [InlineData(null, "valid")]
        public async Task ComparePasswordToHashAsync_ShouldThrow_WhenArgumentsInvalid(string passwordText, string passwordHash)
        {
            Func<Task> act = () => _sut.ComparePasswordToHashAsync(passwordText, passwordHash);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task ComparePasswordToHashAsync_ShouldBeTrue_WhenPasswordsMatch(string passwordText, string passwordHash)
        {
            var result = await _sut.ComparePasswordToHashAsync(passwordText, passwordHash);

            result.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task ComparePasswordToHashAsync_ShouldBeFalse_WhenPasswordsDontMatch(string passwordText, string passwordHash)
        {
            var result = await _sut.ComparePasswordToHashAsync("differentPasswordText", passwordHash);

            result.Should().BeFalse();
        }

        #endregion
    }
}
