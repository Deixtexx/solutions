using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace task07
{
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
}
