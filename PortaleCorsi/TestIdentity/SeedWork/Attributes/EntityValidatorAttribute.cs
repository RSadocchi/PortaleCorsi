using System;

namespace TestIdentity.SeedWork
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityValidatorAttribute : Attribute
    {
        public Type Validator { get; set; }

        public EntityValidatorAttribute(Type validator)
        {
            Validator = validator;
        }
    }
}
