using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Cross;

namespace TFW.WebAPI.Filters
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

            if (operationAttrs.Any() && !operationAttrs.Any(o => allowAnonymousAttr.IsAssignableFrom(o.GetType())))
            {
                var authAttrs = operationAttrs.Where(o => !allowAnonymousAttr.IsAssignableFrom(o.GetType()))
                    .Select(o => o as AuthorizeAttribute);

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
                    [oAuthScheme] = authAttrs.Where(o => o.Policy != null).Select(o => o.Policy).ToArray()
                });
            }
        }
    }
}
