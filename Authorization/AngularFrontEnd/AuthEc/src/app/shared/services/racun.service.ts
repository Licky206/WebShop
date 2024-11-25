import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RacunService {
  
  private apiUrl = 'https://localhost:7164/api/Racun'; // Replace with your actual backend URL

  constructor(private http: HttpClient) {
    
   }

  kreirajRacunSaStavkama(request: any,): Observable<any> {
    return this.http.post(`${this.apiUrl}/KreirajRacunSaStavkama`, request);
  }

  getAllRacuni(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetAllRacuni`);
  }

  getStavkeByRacunId(racunId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetStavkeByRacunId/${racunId}`);
  }
  deleteRacun(racunId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteRacun/${racunId}`);
  }

  updateRacunStatus(racunId: number, newStatus: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/UpdateRacunStatus/${racunId}/${newStatus}`);
  }

  getUkupnaCenaRacuna(racunId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/ukupna-cena/${racunId}`);
  }

  updateStavka(stavkaId: number, newKolicina: number, newPopust: number): Observable<any> {
    if (!stavkaId) {
      console.error('Invalid stavkaId:', stavkaId); // Handle undefined stavkaId
    }
    let params = new HttpParams();
    params = params.append('stavkaId', stavkaId);
    params = params.append('newKolicina', newKolicina);
    params = params.append('newPopust', newPopust);
    
    console.log('Sending update request for Stavka:', params);
  
    return this.http.get(`${this.apiUrl}/update-stavka`, { params: params   });
  }
  
  
}
