﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace TFW.Framework.Validations.Fluent
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
