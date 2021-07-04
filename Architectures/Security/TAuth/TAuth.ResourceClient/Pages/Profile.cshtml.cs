using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TAuth.ResourceClient.Services;

namespace TAuth.ResourceClient.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public ProfileModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public string Address { get; set; } = "Unknown";
        public string FullName { get; set; }

        public async Task OnGetAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfo = await _identityService.GetUserInfoAsync(accessToken);

            FullName = userInfo.FirstOrDefault(o => o.Type == JwtClaimTypes.Name)?.Value;

            var addrVal = userInfo.FirstOrDefault(o => o.Type == JwtClaimTypes.Address)?.Value;
            if (!string.IsNullOrWhiteSpace(addrVal))
            {
                var addressObj = JsonConvert.DeserializeAnonymousType(addrVal, new
                {
                    street_address = "",
                    locality = "",
                    postal_code = 0,
                    country = ""
                });

                Address = string.Join(", ", addressObj.street_address, addressObj.locality, $"{addressObj.postal_code}", addressObj.country);
            }
        }
    }
}
