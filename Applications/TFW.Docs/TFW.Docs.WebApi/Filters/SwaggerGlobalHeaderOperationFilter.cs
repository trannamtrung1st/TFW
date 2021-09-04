using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.Common.Extensions;

namespace TFW.Docs.WebApi.Filters
{
    public class SwaggerGlobalHeaderOperationFilter : IOperationFilter
    {
        private readonly RequestLocalizationOptions _localizationOptions;

        public SwaggerGlobalHeaderOperationFilter(IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _localizationOptions = localizationOptions.Value;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var acceptLanguages = _localizationOptions.SupportedUICultures.Select(
                o => new OpenApiString(o.Name) as IOpenApiAny).ToList();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = DataType.String.ToStringF(),
                    Enum = acceptLanguages
                },
                Description = "Accept Language",
                Required = false
            });
        }
    }
}
