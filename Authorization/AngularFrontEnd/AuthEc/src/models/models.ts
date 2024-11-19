// src/app/models/models.ts

export interface Proizvod {
  ProizvodID: number; // Add this if it is part of your API response
  nazivProizvoda: string;
  cena: number;
  kolicina: number; // Optional, depending on your use case
}
  
export interface StavkeRacuna {
  RacunID: number;
  ProizvodID: number;
  kolicina: number;
  popust: number;
}
  
  export interface Racun {
    racunId: number;
    statusRacuna: string;
    datum: string;
  }

  // request.model.ts
export interface RacunRequest {
  racun: Racun[];
  stavke: StavkeRacuna[];
}