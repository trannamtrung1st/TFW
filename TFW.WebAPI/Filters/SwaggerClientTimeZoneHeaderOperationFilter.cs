using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.Common.Helpers;
using TFW.Framework.Web.Options;

namespace TFW.WebAPI.Filters
{
    public class SwaggerClientTimeZoneHeaderOperationFilter : IOperationFilter
    {
        private readonly HeaderClientTimeZoneProviderOptions _options;

        public SwaggerClientTimeZoneHeaderOperationFilter(IOptions<HeaderClientTimeZoneProviderOptions> options)
        {
            _options = options.Value;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = _options.HeaderName,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = DataType.Number.Name()
                },
                Description = "TimeZoneOffset: the difference of dates, in minutes, " +
                    "between client local time zone and UTC time zone",
                Required = false
            });
        }
    }
}
