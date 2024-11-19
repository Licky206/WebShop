using System.Text.Json.Serialization;

namespace Authorization.Models
{
    public class Proizvod
    {
        public int proizvodID { get; set; }
        public string NazivProizvoda { get; set; }
        public decimal Cena { get; set; }

    }
}
