using PortaleCorsi.Context;
using PortaleCorsi.DbEntities;

namespace PortaleCorsi.Repositories
{
    public class CorsoRepository : RepositoryBase<AppDbContext, CorsoMaster, int>, ICorsoRepository 
    {
        public CorsoRepository(AppDbContext context) : base(context) { }
    }
}
