using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestIdentity
{
    internal class OrSpecification<TEntity> : IOrSpecification<TEntity>
    {
        public ISpecification<TEntity> Spec1 { get; private set; }

        public ISpecification<TEntity> Spec2 { get; private set; }

        internal OrSpecification(ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            Spec1 = spec1 ?? throw new ArgumentNullException("spec1");
            Spec2 = spec2 ?? throw new ArgumentNullException("spec2");
        }

        public Expression<Func<TEntity, bool>> Expression
        {
            get { return Spec1.Expression.Or(Spec2.Expression); }
        }

        public new bool IsSatisfiedBy(TEntity candidate)
        {
            return Spec1.IsSatisfiedBy(candidate) || Spec2.IsSatisfiedBy(candidate);
        }
    }
}
