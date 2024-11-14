import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Proizvod } from '../../../../../models/models';


@Injectable({
  providedIn: 'root'
})
export class ProizvodService {
  private apiUrl = 'https://localhost:7164/api/Proizvodi'; 
  constructor(private http: HttpClient) {}

  getProizvodi(): Observable<Proizvod[]> {
    return this.http.get<Proizvod[]>(this.apiUrl);
  }

  deleteProizvod(proizvodId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteProizvod/${proizvodId}`);
  }

  updateProizvod(proizvod: Proizvod): Observable<void> {
  return this.http.put<void>(`${this.apiUrl}/UpdateProizvod`, proizvod);
}
}
