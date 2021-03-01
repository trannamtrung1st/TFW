using System;
using System.Collections.Generic;
using System.Text;
using TFW.Data.Providers;

namespace TFW.Data
{
    public static class UnitOfWorkManager
    {
        #region static
        private static IUnitOfWorkProvider _uowProvider;
        public static IUnitOfWork Current => _uowProvider.UnitOfWork;

        internal static void Configure(IUnitOfWorkProvider uowProvider)
        {
            if (_uowProvider != null)
                throw new ArgumentNullException($"Already initialized {nameof(uowProvider)}");

            _uowProvider = uowProvider;
        }
        #endregion
    }
}
