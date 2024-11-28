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

  private hasClaim(claim: string): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;

    // Decode JWT token (use a library like 'jwt-decode' or manually decode it)
    const payload = this.decodeToken(token);
    return payload && payload[claim] === 'true'; // Check if the claim exists and is set to 'true'
  }

  // Decode JWT token to read claims (You can use a library like 'jwt-decode' for this)
  private decodeToken(token: string): any {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload));
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
    const headers = this.getHeaders(); // Include token
  
    return this.http.get<boolean>(`${this.apiUrl}/UpdateRacunStatus/${racunId}/${newStatus}`, { headers }).pipe(
      catchError(error => {
        if (error.status === 403) {
          alert('Nemate permisiju za ovu akciju.');
        }
        return throwError(error);
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
      return throwError('Invalid stavkaId');
    }

    // Check if the user has the 'CanEditRacunStatus' claim
    if (!this.hasClaim('CanEditRacunStatus')) {
      alert('Nemate pravo da uredite status računa.');
      return throwError('Unauthorized');
    }

    let params = new HttpParams();
    params = params.append('stavkaId', stavkaId.toString());
    params = params.append('newKolicina', newKolicina.toString());
    params = params.append('newPopust', newPopust.toString());

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
