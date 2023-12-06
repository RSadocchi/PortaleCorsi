using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortaleCorsi.Models
{
    public class CorsoMasterDTO: IValidatableObject
    {
        public int Id { get; set; }

       //ciao
        public DateTime DataCreazione { get; set; }

       
        public string Codice { get; set; } = null!;

      
        public string Nome { get; set; } = null!;

        public string? Descrizione { get; set; }

      
        public DateTime DataInizio { get; set; }

        public bool OnLine { get; set; }

        public string? LuogoLezioni { get; set; }

        public int MaxPartecipanti { get; set; }

    
        public DateTime DataFineIscrizioni { get; set; }

        public bool IscrizioniChiuse { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(string.IsNullOrWhiteSpace(this.Codice))
                yield return new ValidationResult($"Il Codice  è obbligatorio", new[] { nameof(Codice) });

            if(MaxPartecipanti==0 || MaxPartecipanti > 100)
                yield return new ValidationResult($"Valore dei partecipanti errato", new[] { nameof(MaxPartecipanti) });



            //throw new NotImplementedException();
        }
    }
}
