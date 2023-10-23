using IceSync.Infrastructure.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.Tests.Services
{
    [TestFixture]
    public class TokenServiceTests
    {
        private const string TOKEN_VALUE = "SampleToken";
        private TokenService _tokenService;
        private Mock<IMemoryCache> _cacheMock;

        [SetUp]
        public void Setup()
        {
            _cacheMock = new Mock<IMemoryCache>();

            _tokenService = new TokenService(_cacheMock.Object);
        }

        [Test]
        public void GetCurrentToken_WhenTokenIsCached_ShouldReturnToken()
        {
            // Arrange
            object cachedValue = TOKEN_VALUE;
            _cacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedValue))
                      .Returns(true);

            // Act
            var token = _tokenService.GetCurrentToken();

            // Assert
            Assert.AreEqual(TOKEN_VALUE, token);
        }

        [Test]
        public void GetCurrentToken_WhenTokenIsNotCached_ShouldReturnNull()
        {
            // Arrange
            _cacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny)).Returns(false);

            // Act
            var token = _tokenService.GetCurrentToken();

            // Assert
            Assert.IsNull(token);
        }

        [Test]
        public void StoreToken_ShouldCacheTokenWithSlidingExpiration()
        {
            // Arrange
            MemoryCacheEntryOptions actualOptions = null;

            _cacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
                      .Returns(new Func<object, ICacheEntry>((key) =>
                      {
                          var cacheEntryMock = new Mock<ICacheEntry>();
                          cacheEntryMock.SetupSet(entry => entry.SlidingExpiration = It.IsAny<TimeSpan?>())
                                        .Callback<TimeSpan?>(value => actualOptions = new MemoryCacheEntryOptions { SlidingExpiration = value });
                          return cacheEntryMock.Object;
                      }));

            // Act
            _tokenService.StoreToken(TOKEN_VALUE);

            // Assert
            Assert.AreEqual(TimeSpan.FromHours(1), actualOptions.SlidingExpiration);
        }
    }
}
