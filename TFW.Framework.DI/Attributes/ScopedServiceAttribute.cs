using Microsoft.Extensions.DependencyInjection;

namespace TFW.Framework.DI.Attributes
{
    public class ScopedServiceAttribute : ServiceAttribute
    {
        public ScopedServiceAttribute() : base(ServiceLifetime.Scoped)
        {
        }
    }
}
