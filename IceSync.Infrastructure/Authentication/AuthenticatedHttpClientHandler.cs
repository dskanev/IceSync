using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IceSync.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace IceSync.Infrastructure.Authentication
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly IUniLoaderAuthApi _authApi;
        private readonly UniLoaderApiConfiguration _config;
        private readonly TokenService tokenService;

        public AuthenticatedHttpClientHandler(IUniLoaderAuthApi authApi, UniLoaderApiConfiguration config, TokenService tokenService)
        {
            _authApi = authApi;
            _config = config;
            this.tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = tokenService.GetCurrentToken();

            if (token == null || IsJwtTokenExpired(token)) 
            {
                token = await _authApi.Authenticate(new AuthenticateInputModel(_config.CompanyId, _config.UserId, _config.UserSecret));
                tokenService.StoreToken(token);
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }

        public bool IsJwtTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityToken(token);
            var expiration = jwtToken.ValidTo;

            return expiration < DateTime.UtcNow;
        }
    }

}
