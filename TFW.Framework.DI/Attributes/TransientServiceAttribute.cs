using Microsoft.Extensions.DependencyInjection;

namespace TFW.Framework.DI.Attributes
{
    public class TransientServiceAttribute : ServiceAttribute
    {
        public TransientServiceAttribute() : base(ServiceLifetime.Transient)
        {
        }
    }
}
