using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using TFW.Framework.Common.Extensions;
using TFW.Framework.i18n.Options;

namespace TFW.Framework.i18n.Localization.Factory
{
    public class InMemoryStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IOptions<InMemoryLocalizerOptions> _options;
        private readonly MethodInfo _factoryMethod;

        public InMemoryStringLocalizerFactory(IOptions<InMemoryLocalizerOptions> options)
        {
            _options = options;
            _factoryMethod = typeof(InMemoryStringLocalizerFactory)
                .GetInstanceMethod(nameof(CreateGeneric), false, true);
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return _factoryMethod.InvokeGeneric<IStringLocalizer>(this, new[] { resourceSource });
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            throw new NotSupportedException();
        }

        private IStringLocalizer<T> CreateGeneric<T>()
        {
            return new InMemoryLocalizer<T>(_options);
        }
    }
}
