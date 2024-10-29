// src/app/models/models.ts

export interface Proizvod {
    ProizvodID: number;
    NazivProizvoda: string;
    Cena: number;
  }
  
  export interface StavkeRacuna {
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
  