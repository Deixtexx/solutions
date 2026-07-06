using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace task07
{
    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            var displayNameAttr = (DisplayNameAttribute) Attribute.GetCustomAttribute(type, typeof(DisplayNameAttribute));
            var versionAttr = (VersionAttribute) Attribute.GetCustomAttribute(type, typeof(VersionAttribute));

            if (displayNameAttr != null)
                Console.WriteLine($"{type.Name} - {displayNameAttr.DisplayName}");

            if (versionAttr != null)
                Console.WriteLine($"{type.Name} - {versionAttr}");

            var properties = type.GetProperties();
            var methods = type.GetMethods();

            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<DisplayNameAttribute>(true);
                if (attr != null) Console.WriteLine($"{prop.Name} - {attr.DisplayName}");
            }

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<DisplayNameAttribute>(true);
                if (attr != null) Console.WriteLine($"{method.Name} - {attr.DisplayName}");
            }
        }
    }
}
