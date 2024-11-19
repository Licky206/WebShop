import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RacunService {
  
  private apiUrl = 'https://localhost:7164/api/Racun'; // Replace with your actual backend URL

  constructor(private http: HttpClient) { }

  kreirajRacunSaStavkama(request: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/KreirajRacunSaStavkama`, request);
  }
  
}
