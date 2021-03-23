using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TFW.Cross.Models.Common;
using TFW.Cross.Providers;

namespace TFW.Cross
{
    public abstract class BusinessContext
    {
        public abstract PrincipalInfo PrincipalInfo { get; }
        public abstract ClaimsPrincipal User { get; }

        #region Localization
        public abstract IStringLocalizer ResultCodeLocalizer { get; }
        #endregion

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
