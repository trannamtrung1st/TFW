using IdentityModel.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TAuth.ResourceClient.Services
{
    public interface IIdentityService
    {
        Task<IEnumerable<Claim>> GetUserInfoAsync(string accessToken);
    }

    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Claim>> GetUserInfoAsync(string accessToken)
        {
            var metadataResp = await _httpClient.GetDiscoveryDocumentAsync();

            if (metadataResp.IsError)
            {
                throw metadataResp.Exception;
            }

            var userInfoResp = await _httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = metadataResp.UserInfoEndpoint,
                Token = accessToken
            });

            if (userInfoResp.IsError)
            {
                throw userInfoResp.Exception;
            }

            return userInfoResp.Claims;
        }

    }
}
