﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TFW.Framework.Web.Binding
{
    public class QueryObjectModelBinder : ComplexTypeModelBinder
    {
        public QueryObjectModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders,
            ILoggerFactory loggerFactory) : base(propertyBinders, loggerFactory)
        {
        }
    }

    public class QueryObjectModelBinderProvider : IModelBinderProvider
    {
        private static readonly Type[] QueryObjectModelBinderTypes = new[]
        {
            typeof(TimeZoneAwaredDateTimeModelBinder)
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.IsComplexType && context.Metadata.BinderType == typeof(QueryObjectModelBinder))
            {
                var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();

                foreach (var p in context.Metadata.Properties)
                {
                    if (QueryObjectModelBinderTypes.Contains(p.BinderType))
                        propertyBinders.Add(p, context.Services.GetRequiredService(p.BinderType) as IModelBinder);
                    else
                        propertyBinders.Add(p, context.CreateBinder(p));
                }

                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();

                return new QueryObjectModelBinder(propertyBinders, loggerFactory);
            }

            return null;
        }

    }

    public class QueryObjectAttribute : ModelBinderAttribute
    {
        public QueryObjectAttribute()
            : base(typeof(QueryObjectModelBinder))
        {
        }
    }
}
