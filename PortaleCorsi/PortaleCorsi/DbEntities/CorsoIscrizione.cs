using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortaleCorsi.DbEntities
{
    [Table(nameof(CorsoIscrizione))]
    public class CorsoIscrizione
    {
        [Key]
        public int Id { get; set; }
        public int CorsoId { get; set; }
        public int AnagraficaId { get; set; }
        public DateTime DataIscrizione { get; set; }

        public virtual CorsoMaster CorsoMaster { get; set; } = null!;
        public virtual AnagraficaMaster AnagraficaMaster { get; set; } = null!;
    }
}
