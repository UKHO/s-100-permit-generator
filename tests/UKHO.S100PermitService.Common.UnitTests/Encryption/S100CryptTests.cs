﻿using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using UKHO.S100PermitService.Common.Encryption;
using UKHO.S100PermitService.Common.Events;
using UKHO.S100PermitService.Common.Models.ProductKeyService;
using UKHO.S100PermitService.Common.Exceptions;
using UKHO.S100PermitService.Common.Models.UserPermitService;
using UKHO.S100PermitService.Common.Services;

namespace UKHO.S100PermitService.Common.UnitTests.Encryption
{
    [TestFixture]
    public class S100CryptTests
    {
        private const string FakeHardwareId = "FAKE583E6CB6F32FD0B0648AF006A2BD";

        private IAesEncryption _fakeAesEncryption;
        private IManufacturerKeyService _fakeManufacturerKeyService;
        private ILogger<S100Crypt> _fakeLogger;
        private IS100Crypt _s100Crypt;

        [SetUp]
        public void SetUp()
        {
            _fakeAesEncryption = A.Fake<IAesEncryption>();
            _fakeManufacturerKeyService = A.Fake<IManufacturerKeyService>();
            _fakeLogger = A.Fake<ILogger<S100Crypt>>();

            _s100Crypt = new S100Crypt(_fakeAesEncryption, _fakeManufacturerKeyService, _fakeLogger);
        }

        [Test]
        public void WhenParameterIsNull_ThenConstructorThrowsArgumentNullException()
        {
            Action nullAesEncryption = () => new S100Crypt(null, _fakeManufacturerKeyService, _fakeLogger);
            nullAesEncryption.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should()
                .Be("aesEncryption");

            Action nullManufacturerKeyService = () => new S100Crypt(_fakeAesEncryption, null, _fakeLogger);
            nullManufacturerKeyService.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("manufacturerKeyService");

            Action nullLogger = () => new S100Crypt(_fakeAesEncryption, _fakeManufacturerKeyService, null);
            nullLogger.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("logger");
        }

        [Test]
        public void WhenProductKeysDecryptedSuccessfully_ThenReturnsDecryptedKeys()
        {
            var test101ProductKey = "20191817161514131211109876543210";
            var test102ProductKey = "36353433323130292827262524232221";

            A.CallTo(() => _fakeAesEncryption.Decrypt(A<string>.Ignored, A<string>.Ignored))
                                             .Returns(test101ProductKey).Once().Then.Returns(test102ProductKey);

            var result = _s100Crypt.GetDecryptedKeysFromProductKeys(GetProductKeyServiceResponse(), FakeHardwareId);

            result.Should().NotBeNull();
            result.FirstOrDefault().DecryptedKey.Should().Be(test101ProductKey);
            result.LastOrDefault().DecryptedKey.Should().Be(test102ProductKey);

            A.CallTo(_fakeLogger).Where(call =>
                call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Information
                && call.GetArgument<EventId>(1) == EventIds.GetDecryptedKeysFromProductKeysStarted.ToEventId()
                && call.GetArgument<IEnumerable<KeyValuePair<string, object>>>(2)!.ToDictionary(c => c.Key, c => c.Value)["{OriginalFormat}"].ToString() == "Get decrypted keys from product keys started."
            ).MustHaveHappenedOnceExactly();

            A.CallTo(_fakeLogger).Where(call =>

                call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Information
                && call.GetArgument<EventId>(1) == EventIds.GetDecryptedKeysFromProductKeysCompleted.ToEventId()
                && call.GetArgument<IEnumerable<KeyValuePair<string, object>>>(2)!.ToDictionary(c => c.Key, c => c.Value)["{OriginalFormat}"].ToString() == "Get decrypted keys from product keys completed."
            ).MustHaveHappenedOnceExactly();
        }

        private List<ProductKeyServiceResponse> GetProductKeyServiceResponse()
        {
            return
            [
                new()
                {
                    Edition = "1",
                    Key = "01234567891011121314151617181920",
                    ProductName = "test101"
                },
                new()
                {
                    Edition = "1",
                    Key = "21222324252627282930313233343536",
                    ProductName = "test102"
                }
            ];
        }
        private List<ProductKeyServiceResponse> GetInvalidProductKeyServiceResponse()
        {
            return
            [
                new()
                {
                    Edition = "1",
                    Key = "0123456",
                    ProductName = "test101"
                },
                new()
                {
                    Edition = "1",
                    Key = "67891011",
                    ProductName = "test102"
                }
            ];
        }

        [Test]
        public void WhenValidMKeyAndUpnInfo_ThenListOfDecryptedHardwareIdIsReturned()
        {
            const string FakeDecryptedHardwareId = "86C520323CEA3056B5ED7000F98814CB";

            const string FakeMKey = "validMKey12345678901234567890123";

            A.CallTo(() => _fakeManufacturerKeyService.GetManufacturerKeys(A<string>.Ignored)).Returns(FakeMKey);

            A.CallTo(() => _fakeAesEncryption.Decrypt(A<string>.Ignored, A<string>.Ignored)).Returns(FakeDecryptedHardwareId);

            var result = _s100Crypt.GetDecryptedHardwareIdFromUserPermit(GetUpnInfoWithDecryptedHardwareId());

            result.Equals(GetUpnInfo());

            A.CallTo(_fakeLogger).Where(call =>
                call.Method.Name == "Log"
                && call.GetArgument<LogLevel>(0) == LogLevel.Information
                && call.GetArgument<EventId>(1) == EventIds.GetHwIdFromUserPermitStarted.ToEventId()
                && call.GetArgument<IEnumerable<KeyValuePair<string, object>>>(2)!.ToDictionary(c => c.Key, c => c.Value)["{OriginalFormat}"].ToString() == "Get decrypted hardware id from user permits started"
            ).MustHaveHappenedOnceExactly();

            A.CallTo(_fakeLogger).Where(call =>

                call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == LogLevel.Information
                    && call.GetArgument<EventId>(1) == EventIds.GetHwIdFromUserPermitCompleted.ToEventId()
                    && call.GetArgument<IEnumerable<KeyValuePair<string, object>>>(2)!.ToDictionary(c => c.Key, c => c.Value)["{OriginalFormat}"].ToString() == "Get decrypted hardware id from user permits completed"
                ).MustHaveHappenedOnceExactly();
        }

        private static List<UpnInfo> GetUpnInfo()
        {
            return
            [
                new UpnInfo()
                {
                    EncryptedHardwareId = "FE5A853DEF9E83C9FFEF5AA001478103",
                    Upn = "FE5A853DEF9E83C9FFEF5AA001478103DB74C038A1B2C3",
                    MId = "A1B2C3",
                    Crc32 = "DB74C038"
                },
                new UpnInfo()
                {
                    EncryptedHardwareId = "869D4E0E902FA2E1B934A3685E5D0E85",
                    Upn = "869D4E0E902FA2E1B934A3685E5D0E85C1FDEC8BD4E5F6",
                    MId = "D4E5F6",
                    Crc32 = "C1FDEC8B"
                }
            ];
        }

        private static List<UpnInfo> GetUpnInfoWithDecryptedHardwareId()
        {
            return
            [
                new UpnInfo()
                {
                    HardwareId = "86C520323CEA3056B5ED7000F98814CB",
                    EncryptedHardwareId = "FE5A853DEF9E83C9FFEF5AA001478103",
                    Upn = "FE5A853DEF9E83C9FFEF5AA001478103DB74C038A1B2C3",
                    MId = "A1B2C3",
                    Crc32 = "DB74C038"
                },
                new UpnInfo()
                {
                    HardwareId = "B2C0F91ADAAEA51CC5FCCA05C47499E4",
                    EncryptedHardwareId = "869D4E0E902FA2E1B934A3685E5D0E85",
                    Upn = "869D4E0E902FA2E1B934A3685E5D0E85C1FDEC8BD4E5F6",
                    MId = "D4E5F6",
                    Crc32 = "C1FDEC8B"
                }
            ];
        }
    }
}