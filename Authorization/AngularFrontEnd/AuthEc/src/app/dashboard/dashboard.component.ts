import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {
  racunId: number | null = null;
  stavkeRacuna: any[] = [];
  proizvodi: any[] = []; // Array to store products for bulk insert

  constructor(private router: Router, private http: HttpClient) {}

  // Logout function
  onLogout() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('/signup');
  }

  // Method to create a Racun
  kreirajRacun() {
    const proizvodi = [
      { ProizvodID: 1 }, // Example product IDs, replace with actual IDs
      { ProizvodID: 2 }
    ];
    
    this.http.post<any>('http://localhost:5000/KreirajRacun', proizvodi)
      .subscribe(
        response => {
          this.racunId = response.racunId; // Capture the Racun ID
          alert('Racun created successfully!');
        },
        error => console.error('Error creating Racun:', error)
      );
  }

  // Method to bulk insert Proizvod items
  bulkInsertProizvodi() {
    const proizvodi = [
      { ProizvodID: 1, NazivProizvoda: 'Product A', Cena: 100 },
      { ProizvodID: 2, NazivProizvoda: 'Product B', Cena: 200 }
    ];
    
    this.http.post('http://localhost:5000/Dodavanja Proizvoda - bulk insert', proizvodi)
      .subscribe(
        () => alert('Proizvodi bulk insert successful!'),
        error => console.error('Error with bulk insert:', error)
      );
  }

  // Method to get StavkeRacuna by RacunID
  getStavkeRacuna() {
    if (this.racunId) {
      this.http.get<any[]>(`http://localhost:5000/StavkeRacuna/${this.racunId}`)
        .subscribe(
          data => this.stavkeRacuna = data,
          error => console.error('Error fetching StavkeRacuna:', error)
        );
    } else {
      alert('Please create a Racun first.');
    }
  }
}
