using FluentValidation;

namespace TFW.Framework.Validations.Fluent.Extensions
{
    public static class ValidationContextExtensions
    {
        public static bool IsInvokedByMvc<T>(this ValidationContext<T> context)
        {
            return context.RootContextData?.ContainsKey(RootContextDataKey.InvokedByMvc) == true
                && (bool)context.RootContextData[RootContextDataKey.InvokedByMvc] == true;
        }
    }
}
