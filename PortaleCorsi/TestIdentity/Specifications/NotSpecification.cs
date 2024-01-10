using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestIdentity
{
    internal class NotSpecification<T> : INotSpecification<T>
    {
        public ISpecification<T> Inner { get; private set; }

        internal NotSpecification(ISpecification<T> inner)
        {
            Inner = inner ?? throw new ArgumentNullException("spec");
        }

        public Expression<Func<T, bool>> Expression
        {
            get { return Inner.Expression.Not(); }
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return !Inner.IsSatisfiedBy(candidate);
        }
    }

}
