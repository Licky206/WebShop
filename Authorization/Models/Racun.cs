namespace Authorization.Models
{
    public class Racun
    {
        public int RacunId { get; set; }
        public string StatusRacuna { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Vreme { get; set; }

        public List<StavkeRacuna> StavkeRacuna { get; set; } = new List<StavkeRacuna>();

    }
}
