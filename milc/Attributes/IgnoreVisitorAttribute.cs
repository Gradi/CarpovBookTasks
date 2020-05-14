using System;

namespace milc.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class IgnoreVisitorAttribute : Attribute {}
}
