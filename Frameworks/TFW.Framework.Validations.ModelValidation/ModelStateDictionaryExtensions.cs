using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace TFW.Framework.Validations.ModelValidation
{
    public static class ModelStateDictionaryExtensions
    {
        public static ModelError[] GetAllErrors(this ModelStateDictionary modelState, bool includeChildren = false)
        {
            var queue = new Queue<ModelStateEntry>();

            foreach (var kvp in modelState.Values)
                queue.Enqueue(kvp);

            var invalidEntries = new List<ModelStateEntry>();

            while (queue.Count > 0)
            {
                var entry = queue.Dequeue();

                if (entry.ValidationState == ModelValidationState.Invalid)
                    invalidEntries.Add(entry);

                if (includeChildren && entry.Children != null)
                    foreach (var child in entry.Children)
                        queue.Enqueue(child);
            }

            return invalidEntries.SelectMany(entry => entry.Errors).ToArray();
        }
    }
}
