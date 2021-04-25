using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Web.Options;

namespace TFW.Docs.WebApi.Filters
{
    public class SwaggerTimeZoneHeaderOperationFilter : IOperationFilter
    {
        private readonly HeaderClientTimeZoneProviderOptions _headerClientOptions;
        private readonly HeaderTimeZoneProviderOptions _headerOptions;
        private readonly RequestTimeZoneOptions _requestTzOptions;

        public SwaggerTimeZoneHeaderOperationFilter(IOptions<HeaderClientTimeZoneProviderOptions> headerClientOptions,
            IOptions<HeaderTimeZoneProviderOptions> headerOptions,
            IOptions<RequestTimeZoneOptions> requestTzOptions)
        {
            _headerClientOptions = headerClientOptions.Value;
            _headerOptions = headerOptions.Value;
            _requestTzOptions = requestTzOptions.Value;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = _headerClientOptions.HeaderName,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = DataType.Number.ToStringF()
                },
                Description = "TimeZoneOffset: the difference of dates, in minutes, " +
                    "between client local time zone and UTC time zone",
                Required = false
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = _headerOptions.HeaderName,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = DataType.String.ToStringF()
                },
                Description = "Send a TimeZoneId supported by the application",
                Required = false
            });
        }
    }
}
