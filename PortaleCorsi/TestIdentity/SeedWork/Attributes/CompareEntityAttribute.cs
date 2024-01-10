using System;

namespace TestIdentity.SeedWork
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CompareEntityAttribute : Attribute { }
}
