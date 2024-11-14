import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StavkeRacuna } from '../../models/models';  // Adjust the path if necessary

@Injectable({
    providedIn: 'root'
  })
  export class StavkeRacunaService {
    private apiUrl = 'https://localhost:7164/api/DodajStavkeRacuna';  // Base URL for your API
  
    constructor(private http: HttpClient) {}
  
    addStavkeRacuna(racunId: number, stavke: StavkeRacuna[]): Observable<any> {
      const url = `${this.apiUrl}?racunId=${racunId}`;
      return this.http.post<any>(url, stavke);
    }
}
