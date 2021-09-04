using System;

namespace TFW.Framework.Common.Extensions
{
    public static class DelegateExtensions
    {
#nullable enable
        public static (object?, Exception?) SafeCall(this Delegate dlg, params object?[]? args)
        {
            try
            {
                return (dlg.DynamicInvoke(args), null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

    }
}
