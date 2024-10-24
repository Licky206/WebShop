namespace Authorization.Models
{
    public class StavkeRacuna
    {
        public int StavkeRacunaID { get; set; }
        public int RacunID { get; set; }
        public int Kolicina { get; set; }
        public decimal? Popust { get; set; }

        public Proizvod Proizvod { get; set; }
    }
}
