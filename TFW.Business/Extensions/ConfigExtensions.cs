using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Business.Logics;
using TFW.Business.Services;
using TFW.Cross.Profiles;
using TFW.Framework.AutoMapper;

namespace TFW.Business.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection ConfigureBusiness(this IServiceCollection services)
        {
            //AutoMapper
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AppUserProfile>();
            });
            GlobalMapper.Instance = mapConfig.CreateMapper();

            var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes());

            var baseLogicType = typeof(BaseLogic);
            var iLogicType = typeof(ILogic);
            var logicTypes = assemblyTypes
                .Where(t => baseLogicType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
            var logicInterfaces = assemblyTypes
                .Where(t => iLogicType.IsAssignableFrom(t) && t.IsInterface && t != iLogicType);
            foreach (var t in logicTypes)
            {
                var iType = logicTypes.First(itf => itf.IsAssignableFrom(t));
                services.AddScoped(iType, t);
            }

            var baseServiceType = typeof(BaseService);
            var iServiceType = typeof(IService);
            var serviceTypes = assemblyTypes
                .Where(t => baseServiceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
            var serviceInterfaces = assemblyTypes
                .Where(t => iServiceType.IsAssignableFrom(t) && t.IsInterface && t != iServiceType);
            foreach (var t in serviceTypes)
            {
                var iType = serviceInterfaces.First(itf => itf.IsAssignableFrom(t));
                services.AddScoped(iType, t);
            }

            return services;
        }
    }
}
