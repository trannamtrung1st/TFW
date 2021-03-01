using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TFW.WebAPI
{
    public static class WebApiConsts
    {
        public static readonly IEnumerable<string> ExcludedAssemblyDirs = ImmutableArray.Create("runtimes");
    }
}
