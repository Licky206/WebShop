import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Proizvod } from '../../../../../models/models';

@Injectable({
  providedIn: 'root'
})
export class ProizvodService {
  private apiUrl = 'https://localhost:7164/api/Proizvod/PROIZVODI'; 
  
  constructor(private http: HttpClient) {}

  getProizvodi(): Observable<Proizvod[]> {
    const token = localStorage.getItem('token'); // Retrieve token from localStorage
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Add token in the Authorization header
    });
    
    return this.http.get<Proizvod[]>(this.apiUrl, { headers });
  }

  deleteProizvod(proizvodId: number): Observable<void> {
    const token = localStorage.getItem('token'); // Retrieve token from localStorage
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Add token in the Authorization header
    });

    return this.http.delete<void>(`${this.apiUrl}/DeleteProizvod/${proizvodId}`, { headers });
  }

  updateProizvod(proizvod: Proizvod): Observable<void> {
    const token = localStorage.getItem('token'); // Retrieve token from localStorage
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Add token in the Authorization header
    });

    return this.http.put<void>(`${this.apiUrl}/UpdateProizvod`, proizvod, { headers });
  }
}
