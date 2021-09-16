using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAuth.IDP;

namespace IdentityServerHost.Quickstart.UI
{
    public class SetupOTPAppViewModel
    {
        [Required]
        public string SecretKey { get; set; }
        public string QrCodeSetupImageUrl { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
