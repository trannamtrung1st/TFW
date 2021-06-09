using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TFW.Framework.i18n.Helpers
{
    public static class CultureHelper
    {
        public static CultureInfo[] GetCultures(CultureTypes type)
        {
            return CultureInfo.GetCultures(type);
        }

        public static RegionInfo[] GetDistinctRegions()
        {
            var regions = GetCultures(CultureTypes.SpecificCultures & ~CultureTypes.NeutralCultures)
                .Where(o => !o.IsNeutralCulture).Select(o => o.LCID)
                .Distinct()
                .Select(o => ToRegionInfo(o)).Where(o => o != null)
                .Distinct().ToArray();

            return regions;
        }

        public static RegionInfo ToRegionInfo(int lcid)
        {
            try
            {
                return new RegionInfo(lcid);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
