﻿using System.Text.Json.Serialization;

namespace Authorization.Models
{
    public class Proizvod
    {
        public int ProizvodID { get; set; }
        public string NazivProizvoda { get; set; }
        public decimal Cena { get; set; }
        public int Kolicina { get; set; }

    }
}
