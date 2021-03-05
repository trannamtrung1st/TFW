﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFW.Framework.Validations.ModelValidation.Helpers
{
    public static class ModelValidationHelper
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