using System;

namespace TestIdentity.SeedWork
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityTrackedAttribute : Attribute
    {
        public EntityTrackedAttribute()
        {

        }
    }
}
