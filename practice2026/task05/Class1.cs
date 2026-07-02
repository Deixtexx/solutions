namespace task05
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;

    public class ClassAnalyzer
    {
        private Type _type;

        public ClassAnalyzer(Type type)
        {
            _type = type;
        }

        public IEnumerable<string> GetPublicMethods()
        {
            return _type.GetMethods()
                        .Select(m => m.Name);
        }

        public IEnumerable<string?> GetMethodParams(string methodname) 
        {
            MethodInfo? info = _type.GetMethod(methodname);
            return info.GetParameters()
                       .Select(p => p.Name)
                       .Append(info.ReturnType.Name); //тип возвращаемого значения
        }

        public IEnumerable<string> GetAllFields()
        {
            BindingFlags flags = BindingFlags.Public |
                                 BindingFlags.NonPublic |
                                 BindingFlags.Instance;
            return _type.GetFields(flags)
                        .Select(m => m.Name);
        }

        public IEnumerable<string> GetProperties()
        {
            return _type.GetProperties()
                        .Select(m => m.Name);
        }

        public bool HasAttribute<T>() where T : Attribute
        {
            return _type.IsDefined(typeof(T), true);
        }
    }
}
