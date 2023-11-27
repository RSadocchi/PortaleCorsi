using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortaleCorsi.DbEntities
{
    [Table(nameof(CorsoLezione))]
    public class CorsoLezione
    {
        [Key]
        public int Id { get; set; }

        public int CorsoId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DataOraInizio { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DataOraFine { get; set; }

        public string Descrizione { get; set; } = null!;

        public virtual CorsoMaster CorsoMaster { get; set; } = null!;
    }
}
