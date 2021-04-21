using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFW.Docs.Cross
{
    public static class Settings
    {
        static Settings()
        {
            SettingsMap = new Dictionary<Type, object>();
        }

        public static IDictionary<Type, object> SettingsMap { get; set; }

        public static void Set<T>(T setting)
        {
            SettingsMap[typeof(T)] = setting;
        }

        public static T Get<T>() where T : class
        {
            return SettingsMap[typeof(T)] as T;
        }
    }
}
