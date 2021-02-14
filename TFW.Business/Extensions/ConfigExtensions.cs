﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TFW.Business.Logics;
using TFW.Business.Services;
using TFW.Cross.Profiles;
using TFW.Framework.AutoMapper;
using TFW.Framework.DI;

namespace TFW.Business.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection ConfigureBusiness(this IServiceCollection services)
        {
            // configure Business 
            return services;
        }
    }
}