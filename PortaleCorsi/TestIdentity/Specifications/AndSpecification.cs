using System;
using System.Linq.Expressions;

namespace TestIdentity
{
    internal class AndSpecification<T> : IAndSpecification<T>
    {
        public ISpecification<T> Spec1 { get; private set; }

        public ISpecification<T> Spec2 { get; private set; }

        internal AndSpecification(ISpecification<T> spec1, ISpecification<T> spec2)
        {
            Spec1 = spec1 ?? throw new ArgumentNullException("spec1");
            Spec2 = spec2 ?? throw new ArgumentNullException("spec2");
        }

        public Expression<Func<T, bool>> Expression { get { return Spec1.Expression.And(Spec2.Expression); } }

        public bool IsSatisfiedBy(T candidate) { return Spec1.IsSatisfiedBy(candidate) && Spec2.IsSatisfiedBy(candidate); }
    }
}
