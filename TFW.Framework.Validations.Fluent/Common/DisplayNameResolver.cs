using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TFW.Framework.Validations.Fluent.Common
{
    public static class DisplayNameResolver
    {
        public static string Resolve(Type type, MemberInfo memberInfo, LambdaExpression expr)
        {
            if (memberInfo == null) return null;

            var displayName = memberInfo.GetCustomAttribute<DisplayAttribute>()?.Name;

            if (displayName == null)
                displayName = memberInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;

            return displayName ?? memberInfo.Name;
        }
    }
}
