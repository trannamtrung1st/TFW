using Microsoft.Extensions.Localization;

namespace TFW.Framework.Validations.Fluent
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
