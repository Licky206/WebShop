namespace Authorization.Models
{
    public class Korpa
    {
        public List<StavkaKorpe> Stavke { get; set; } = new List<StavkaKorpe>();


        public void DodajStavku(StavkaKorpe stavka)
        {
            Stavke.Add(stavka);
        }
        public void OcistiKorpu()
        {
            Stavke.Clear();
        }
    }
}
