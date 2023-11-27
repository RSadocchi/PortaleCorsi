using PortaleCorsi.DbEntities;
using PortaleCorsi.Models;
using PortaleCorsi.Repositories;

namespace PortaleCorsi.Servizi
{
    public class CorsoService
    {
        readonly ILogger<CorsoService> logger;

        readonly IConfiguration configuration;

        readonly ICorsoRepository repository;


        public CorsoService(ILogger<CorsoService> logger, IConfiguration configuration, ICorsoRepository repository)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.repository = repository;
        }

        public async Task<CorsoMaster?> CreacorsoAsync(CorsoMasterDTO model)
        {
            if (model == null)
            {
                return null;
            }
            var corso = new CorsoMaster()
            {
                Codice = model.Codice,
                Nome = model.Nome,
                DataCreazione = model.DataCreazione,
                Descrizione = model.Descrizione,    
                LuogoLezioni=model.LuogoLezioni,
                MaxPartecipanti = model.MaxPartecipanti,
                DataInizio= model.DataInizio,
                DataFineIscrizioni = model.DataFineIscrizioni,
                OnLine = model.OnLine,
                IscrizioniChiuse = model.IscrizioniChiuse,


            };
              corso=await repository.AddAsync(corso);
            await repository.UnitOfWork.SaveEntitiesAsync();   // salva i record nel database
        
            return corso;
        }
    }
}
