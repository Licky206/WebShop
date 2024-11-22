import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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
    
}
