import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Proizvod, StavkeRacuna } from '../../models/models';  
import { CommonModule } from '@angular/common'; 

 // Adjust imports based on your structure

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  //styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  availableProizvodi: Proizvod[] = [];  
  stavkeRacuna: StavkeRacuna[] = [];    
  racunId: number | null = null;       
  cart: Proizvod[] = [];               

  constructor(private http: HttpClient, private router : Router) {}

  ngOnInit() {
    this.fetchAvailableProizvodi();
  }

  // Fetch available products
  fetchAvailableProizvodi() {
    this.http.get<Proizvod[]>('/api/your-endpoint-for-products').subscribe(
      data => this.availableProizvodi = data,
      error => console.error('Error fetching products', error)
    );
  }

  // Add selected product to cart
  addToRacun(proizvod: Proizvod) {
    this.cart.push(proizvod); // Adds product to the cart array
  }

  // Create Racun with cart items
  kreirajRacun() {
    this.http.post<any>('https://localhost:7164/api/KreirajRacun', this.cart).subscribe(
      response => {
        this.racunId = response.id; // Store Racun ID from response
        this.stavkeRacuna = response.stavkeRacuna; // If response includes items, display them
        this.cart = []; // Clear cart after Racun is created
      },
      error => console.error('Error creating Racun', error)
    );
  }

  // Bulk insert products (optional if bulk operation needed)
  bulkInsertProizvodi() {
    this.http.post<any>('https://localhost:7164/api/DodavanjaProizvodaBulkInsert', this.availableProizvodi).subscribe(
      () => console.log('Bulk insert successful'),
      error => console.error('Bulk insert error', error)
    );
  }

  // Get Racun items based on Racun ID
  getStavkeRacuna() {
    this.http.get<StavkeRacuna[]>(`/api/StavkeRacuna/${this.racunId}`).subscribe(
      data => this.stavkeRacuna = data,
      error => console.error('Error fetching Stavke Racuna', error)
    );
  }
  onLogout() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('/signup');
  }
}
