using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TFW.Framework.AutoMapper;

namespace TFW.Cross.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection ConfigureCross(this IServiceCollection services)
        {
            // configure Cross
            return services;
        }
    }
}
