using System;

namespace TestIdentity.SeedWork
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ContextKeyAttribute : Attribute { }
}
