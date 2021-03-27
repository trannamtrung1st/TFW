﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.DI.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute()
        {
        }

    }
}
