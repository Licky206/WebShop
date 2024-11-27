import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RacunService {
  
  private apiUrl = 'https://localhost:7164/api/Racun'; // Replace with your actual backend URL

  constructor(private http: HttpClient) {}
  errorMessage: string | null = null;

  private getHeaders() {
    const token = localStorage.getItem('token'); // Retrieve token from localStorage
    return new HttpHeaders({
      'Authorization': `Bearer ${token}` // Add token in the Authorization header
    });
  }

  kreirajRacunSaStavkama(request: any): Observable<any> {
    const headers = this.getHeaders(); // Get headers with token
    return this.http.post(`${this.apiUrl}/KreirajRacunSaStavkama`, request, { headers });
  }

  getAllRacuni(): Observable<any[]> {
    const headers = this.getHeaders(); // Get headers with token
    return this.http.get<any[]>(`${this.apiUrl}/GetAllRacuni`, { headers });
  }

  getStavkeByRacunId(racunId: number): Observable<any[]> {
    const headers = this.getHeaders(); // Get headers with token
    return this.http.get<any[]>(`${this.apiUrl}/GetStavkeByRacunId/${racunId}`, { headers });
  }

  deleteRacun(racunId: number): Observable<any> {
    const headers = this.getHeaders(); // Get headers with token
    return this.http.delete(`${this.apiUrl}/DeleteRacun/${racunId}`, { headers });
  }

  updateRacunStatus(racunId: number, newStatus: string): Observable<boolean> {
    const headers = this.getHeaders(); // Get headers with token
  
    return this.http.get<boolean>(`${this.apiUrl}/UpdateRacunStatus/${racunId}/${newStatus}`, { headers }).pipe(
      catchError(error => {
        if (error.status === 403) {
          alert('Niste admin, ne možete to raditi.');
        }
        return throwError(error); // Propagate the error for further handling if needed
      })
    );
  }
  
  getUkupnaCenaRacuna(racunId: number): Observable<any> {
    const headers = this.getHeaders(); // Get headers with token
    return this.http.get<any>(`${this.apiUrl}/ukupna-cena/${racunId}`, { headers });
  }

  updateStavka(stavkaId: number, newKolicina: number, newPopust: number): Observable<any> {
    if (!stavkaId) {
      console.error('Invalid stavkaId:', stavkaId);
    }

    let params = new HttpParams();
    params = params.append('stavkaId', stavkaId);
    params = params.append('newKolicina', newKolicina);
    params = params.append('newPopust', newPopust);

    const headers = this.getHeaders();

    return this.http.get(`${this.apiUrl}/update-stavka`, { params, headers }).pipe(
      catchError(error => {
        if (error.status === 403) {
          alert('Niste admin, ne možete to raditi.');
        }
        return throwError(error); // Propagate error for further handling
      })
    );
  }
}
