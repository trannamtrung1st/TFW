using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Validators.Identity;
using TFW.Framework.i18n.Localization;

namespace TFW.Cross.Resources.Validators.Identity
{
    public class RegisterModelValidatorResources : IInMemoryResources
    {
        public RegisterModelValidatorResources()
        {
            Resources = new Dictionary<string, IDictionary<string, string>>();
            Resources[string.Empty] = new Dictionary<string, string>()
            {
                { RegisterModelValidator.Message.ConfirmPasswordDoesNotMatch, "Confirmation password does not match" }
            };
            Resources["vi"] = new Dictionary<string, string>()
            {
                { RegisterModelValidator.Message.ConfirmPasswordDoesNotMatch, "Mật khẩu xác nhận không chính xác" }
            };
        }

        public Type SourceType => typeof(RegisterModelValidator);

        public IDictionary<string, IDictionary<string, string>> Resources { get; }
    }
}
