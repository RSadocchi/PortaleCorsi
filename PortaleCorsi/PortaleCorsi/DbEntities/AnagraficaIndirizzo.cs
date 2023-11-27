namespace PortaleCorsi.DbEntities
{
    public class AnagraficaIndirizzo
    {
        public int Id { get; set; }
        public int AnagraficaId { get; set; }
        public string Indirizzo { get; set; } = null!;
        public string Civico { get; set; } = null!;
        public string Citta { get; set; } = null!;
        public string Cap { get; set; } = null!;
        public string Prov { get; set; } = null!;
        public string? Alias { get; set; }

        public virtual AnagraficaMaster AnagraficaMaster { get; set; } = null!;
    }
}
