using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortaleCorsi.DbEntities
{
    [Table(nameof(CorsoMaster))]
    public class CorsoMaster
    {
        [Key]
        public int Id { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime DataCreazione { get; set; }
        
        [Required, MaxLength(25)]
        public string Codice { get; set; } = null!;
        
        [Required, MaxLength(100)]
        public string Nome { get; set; } = null!;
        
        public string? Descrizione { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime DataInizio { get; set; }
        
        public bool OnLine { get; set; }
        
        public string? LuogoLezioni { get; set; }
        
        public int MaxPartecipanti { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DataFineIscrizioni { get; set; }

        public bool IscrizioniChiuse { get; set; }

        public virtual ICollection<CorsoLezione> Lezioni { get; set; } = new HashSet<CorsoLezione>();
        public virtual ICollection<CorsoIscrizione> Iscrizioni { get; set; } = new HashSet<CorsoIscrizione>();
    }
}
