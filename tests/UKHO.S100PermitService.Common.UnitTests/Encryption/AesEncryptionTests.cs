﻿using FluentAssertions;
using UKHO.S100PermitService.Common.Encryption;

namespace UKHO.S100PermitService.Common.UnitTests.Encryption
{
    [TestFixture]
    public class AesEncryptionTests
    {
        private const string FakeText = "00112233445566778899AABBCCDDEEFF";
        private const string FakeKey = "000102030405060708090A0B0C0D0E0F";
        private IAesEncryption _aesEncryption;

        [SetUp]
        public void SetUp()
        {
            _aesEncryption = new AesEncryption();
        }

        [Test]
        public void WhenProvidedValidData_ThenSuccessfullyReturnsDecryptedData()
        {
            var result = _aesEncryption.Decrypt(FakeText, FakeKey);

            result.Should().NotBeNullOrEmpty();
            result.Should().NotBe(FakeText);
        }

        [Test]
        public void WhenProvidedValidData_ThenSuccessfullyReturnsEncryptedData()
        {
            var result = _aesEncryption.Encrypt(FakeText, FakeKey);

            result.Should().NotBeNullOrEmpty();
            result.Should().NotBe(FakeText);
        }
    }
}