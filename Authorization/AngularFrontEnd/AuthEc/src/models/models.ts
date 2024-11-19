// src/app/models/models.ts

export interface Proizvod {
  proizvodID: number; // Add this if it is part of your API response
  nazivProizvoda: string;
  cena: number;
}
  
export interface StavkeRacuna {
  RacunID: number;
  proizvodID: number;
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