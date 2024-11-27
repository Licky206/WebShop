import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RacunRequest, StavkeRacuna } from '../../models/models';  // Adjust the path if necessary

@Injectable({
  providedIn: 'root'
})
export class StavkeRacunaService {
  private apiUrl = 'https://localhost:7164/api/Racun';  // Correct URL

  constructor(private http: HttpClient) {}

  private getHeaders() {
    const token = localStorage.getItem('token'); // Retrieve token from localStorage
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,  // Add token in the Authorization header
      'Content-Type': 'application/json'   // Specify the content type as JSON
    });
  }

  kreirajRacunSaStavkama(racunRequest: RacunRequest): Observable<any> {
    const headers = this.getHeaders(); // Get headers with token
    return this.http.post<any>(`${this.apiUrl}/KreirajRacunSaStavkama`, racunRequest, { headers });
  }
}
