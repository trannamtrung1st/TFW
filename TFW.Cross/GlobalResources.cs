using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text;

namespace TFW.Cross
{
    public static class GlobalResources
    {
        public static IMapper Mapper { get; set; }
        public static IDynamicLinkCustomTypeProvider CustomTypeProvider { get; set; }
    }
}
