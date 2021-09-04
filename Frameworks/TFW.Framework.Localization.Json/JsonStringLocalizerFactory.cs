using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using TFW.Framework.Common.Extensions;

namespace TFW.Framework.Localization.Json
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IOptions<JsonLocalizerOptions> _options;
        private readonly MethodInfo _factoryMethod;

        public JsonStringLocalizerFactory(IOptions<JsonLocalizerOptions> options)
        {
            _options = options;
            _factoryMethod = typeof(JsonStringLocalizerFactory)
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
            return new JsonLocalizer<T>(_options);
        }
    }
}
