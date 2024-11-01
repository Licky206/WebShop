namespace Authorization.Models
{
    public class StavkaKorpe
    {
        public int ProizvodID { get; set; }
        public string NazivProizvoda { get; set; }
        public decimal Cena { get; set; }
        public int Kolicina { get; set; }
        public decimal Popust { get; set; }
    }
}
