using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Bindings
{
    public class UtcDateTimeModelBinder : IModelBinder
    {
        private static readonly Type[] supportedTypes = new Type[] { typeof(DateTime), typeof(DateTime?) };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (!supportedTypes.Contains(bindingContext.ModelType))
            {
                return Task.CompletedTask;
            }

            var modelName = GetModelName(bindingContext);
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            var dateToParse = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(dateToParse))
            {
                return Task.CompletedTask;
            }

            if (DateTime.TryParse(dateToParse, CultureInfo.InvariantCulture, styles: DateTimeStyles.AdjustToUniversal, out var dateTime))
                bindingContext.Result = ModelBindingResult.Success(dateTime);

            return Task.CompletedTask;
        }

        private string GetModelName(ModelBindingContext bindingContext)
        {
            if (!string.IsNullOrEmpty(bindingContext.BinderModelName))
            {
                return bindingContext.BinderModelName;
            }

            return bindingContext.ModelName;
        }
    }

    public class UtcDateTimeAttribute : ModelBinderAttribute
    {
        public UtcDateTimeAttribute() : base(typeof(UtcDateTimeModelBinder))
        {
        }
    }
}
