using Microsoft.Extensions.Localization;
using TFW.Framework.Validations.Fluent.Providers;

namespace TFW.Framework.Validations.Fluent.Validators
{
    public class LocalizedSafeValidator<T, Context> : SafeValidator<T>
    {
        protected readonly IStringLocalizer<Context> localizer;

        public LocalizedSafeValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<Context> localizer) : base(validationResultProvider)
        {
            this.localizer = localizer;
        }
    }
}
