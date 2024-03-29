﻿using System;
using System.Collections.Generic;

namespace TFW.Framework.i18n.Options
{
    public class InMemoryLocalizerOptions
    {
        public InMemoryLocalizerOptions()
        {
            Resources = new Dictionary<Type, IDictionary<string, IDictionary<string, string>>>();
        }

        public IDictionary<Type, IDictionary<string, IDictionary<string, string>>> Resources { get; set; }
    }
}
