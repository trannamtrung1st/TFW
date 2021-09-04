using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TFW.Docs.Cross;

namespace TFW.Docs.WebApi.Filters
{
    public class SwaggerSecurityOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var allowAnonymousAttr = typeof(AllowAnonymousAttribute);
            Func<object, bool> filter = (object o) => typeof(AuthorizeAttribute).IsAssignableFrom(o.GetType()) ||
                allowAnonymousAttr.IsAssignableFrom(o.GetType());

            var operationAttrs = context.MethodInfo
                .GetCustomAttributes(true)
                .Where(filter).Distinct();

            if (!operationAttrs.Any())
                operationAttrs = context.MethodInfo.ReflectedType
                    .GetCustomAttributes(true)
                    .Where(filter).Distinct();

            operationAttrs = operationAttrs.ToArray();

            if (operationAttrs.Any() && !operationAttrs.OfType<AllowAnonymousAttribute>().Any())
            {
                var authAttrs = operationAttrs.OfType<AuthorizeAttribute>();

                operation.Responses.Add($"{(int)HttpStatusCode.Unauthorized}", new OpenApiResponse
                {
                    Description = nameof(HttpStatusCode.Unauthorized)
                });
                operation.Responses.Add($"{(int)HttpStatusCode.Forbidden}", new OpenApiResponse
                {
                    Description = nameof(HttpStatusCode.Forbidden)
                });

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                };

                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    //[oAuthScheme] = authAttrs.Where(o => o.Policy != null).Select(o => o.Policy).ToArray()
                    [oAuthScheme] = new string[0]
                });

                if (authAttrs.Any(attr =>
                    attr.AuthenticationSchemes?
                        .Split(',').Any(scheme => scheme.Equals(
                            SecurityConsts.ClientAuthenticationScheme, StringComparison.OrdinalIgnoreCase)) == true))
                {
                    var clientScheme = new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecurityConsts.ClientAuthenticationScheme
                        }
                    };

                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [clientScheme] = new string[0]
                    });
                }
            }
        }
    }
}
