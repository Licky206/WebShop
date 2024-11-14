// src/app/models/models.ts

export interface Proizvod {
  ProizvodID: number; // Add this if it is part of your API response
  nazivProizvoda: string;
  cena: number;
  Kolicina: number; // Optional, depending on your use case
}
  
  export interface StavkeRacuna {
    proizvod : Proizvod;
    StavkeRacunaID?: number;
    RacunID: number;
    Kolicina: number;
    Popust: number;
    ProizvodID: number;
    Proizvod?: Proizvod;
  }
  
  export interface Racun {
    RacunId?: number;
    StatusRacuna: string;
    Datum: Date;
    Vreme: string;
    StavkeRacuna?: StavkeRacuna[];
  }
  