using Microsoft.Extensions.DependencyInjection;

namespace TFW.Framework.DI.Attributes
{
    public class SingletonServiceAttribute : ServiceAttribute
    {
        public SingletonServiceAttribute() : base(ServiceLifetime.Singleton)
        {
        }
    }
}
