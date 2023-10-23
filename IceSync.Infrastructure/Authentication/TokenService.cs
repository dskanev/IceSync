using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.Authentication
{
    public class TokenService
    {
        private const string TOKEN_KEY = "UNILOADER_TOKEN";
        private readonly IMemoryCache _cache;

        public TokenService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string GetCurrentToken()
        {
            _cache.TryGetValue(TOKEN_KEY, out string token);
            return token;
        }

        public void StoreToken(string token)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                // Set a sliding expiration to remove the token if not accessed for a given time (e.g., 1 hour).
                // Adjust as needed.
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            _cache.Set(TOKEN_KEY, token, cacheEntryOptions);
        }
    }
}
