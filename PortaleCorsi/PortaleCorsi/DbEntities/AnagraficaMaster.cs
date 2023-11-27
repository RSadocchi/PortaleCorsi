using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortaleCorsi.DbEntities
{
    public class AnagraficaMaster
    {
        public int Id { get; set; }
        public string CodiceFiscale { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Cognome { get; set; } = null!;

        public virtual ICollection<AnagraficaIndirizzo> Indirizzi { get; set; } = new HashSet<AnagraficaIndirizzo>();
        public virtual ICollection<CorsoIscrizione> Iscrizioni { get; set; } = new HashSet<CorsoIscrizione>();
    }
}
