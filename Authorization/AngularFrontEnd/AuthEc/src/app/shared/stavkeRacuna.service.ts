import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RacunRequest, StavkeRacuna } from '../../models/models';  // Adjust the path if necessary

@Injectable({
    providedIn: 'root'
  })
  export class StavkeRacunaService {
    private apiUrl = 'https://localhost:7164/api/Racun';  // Ispravan URL

  
    constructor(private http: HttpClient) {

      
    }
  

    kreirajRacunSaStavkama(racunRequest: RacunRequest): Observable<any> {
      const headers = new HttpHeaders().set('Content-Type', 'application/json');
      return this.http.post<any>(`${this.apiUrl}/KreirajRacunSaStavkama`, racunRequest, { headers }); // Ispravan URL sa 'api/Racun'
    }
}
