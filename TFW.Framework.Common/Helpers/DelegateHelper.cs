using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Common.Helpers
{
    public static class DelegateHelper
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
