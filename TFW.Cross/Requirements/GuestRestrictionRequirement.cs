using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Web.Requirements;

namespace TFW.Cross.Requirements
{
    public class GuestRestrictionRequirement : IGroupRequirement
    {
        public string Group { get; }
        public TimeSpan MustBefore { get; }

        public GuestRestrictionRequirement(TimeSpan mustBefore, string group = null)
        {
            MustBefore = mustBefore;
            Group = group;
        }
    }
}
