using PortaleCorsi.Context;
using PortaleCorsi.DbEntities;

namespace PortaleCorsi.Repositories
{
    public class AnagraficaRepository : RepositoryBase<AppDbContext, AnagraficaMaster, int>, IAnagraficaRepository
    {
        public AnagraficaRepository(AppDbContext context) : base(context) { }
    }
}
