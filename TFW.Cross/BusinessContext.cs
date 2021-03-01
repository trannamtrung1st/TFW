using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Common;
using TFW.Cross.Providers;

namespace TFW.Cross
{
    public abstract class BusinessContext
    {
        public abstract PrincipalInfo PrincipalInfo { get; }

        #region static
        private static IBusinessContextProvider _bizContextProvider;
        public static BusinessContext Current => _bizContextProvider.BusinessContext;

        internal static void Configure(IBusinessContextProvider bizContextProvider)
        {
            if (_bizContextProvider != null)
                throw new ArgumentNullException($"Already initialized {nameof(bizContextProvider)}");

            _bizContextProvider = bizContextProvider;
        }
        #endregion
    }
}
