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
}
