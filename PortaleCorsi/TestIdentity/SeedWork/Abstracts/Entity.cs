using System;

namespace TestIdentity.SeedWork
{
    public abstract class Entity
    {
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity)) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null)) return (Object.Equals(right, null)) ? true : false;
            else return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
