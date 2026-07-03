using System.Reflection;

namespace task07
{
    [AttributeUsage(AttributeTargets.All)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }
        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class VersionAttribute : Attribute
    {
        public int Major { get; }
        public int Minor { get; }

        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }

        public override string ToString() => $"{Major}.{Minor}";
    }

    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            var displayNameAttr = (DisplayNameAttribute) Attribute.GetCustomAttribute(type, typeof(DisplayNameAttribute));
            var versionAttr = (VersionAttribute) Attribute.GetCustomAttribute(type, typeof(VersionAttribute));

            if (displayNameAttr != null) 
                Console.WriteLine(displayNameAttr.DisplayName);

            if (versionAttr != null)
                Console.WriteLine(versionAttr);

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
