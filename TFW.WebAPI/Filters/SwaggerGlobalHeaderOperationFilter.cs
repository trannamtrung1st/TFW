using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Common.Helpers;

namespace TFW.WebAPI.Filters
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
                o => new OpenApiString(o.TwoLetterISOLanguageName) as IOpenApiAny).ToList();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = DataType.String.Name(),
                    Enum = acceptLanguages
                },
                Description = "Accept Language",
                Required = false
            });
        }
    }
}
