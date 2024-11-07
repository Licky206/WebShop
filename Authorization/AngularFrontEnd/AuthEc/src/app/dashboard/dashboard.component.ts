import { Component, input, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Proizvod, StavkeRacuna } from '../../models/models';
import { ProizvodService } from '../shared/services/proizvod/services/proizvod.service';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [FormsModule, CommonModule],
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']  

})
export class DashboardComponent implements OnInit {
    availableProizvodi: Proizvod[] = [];
    stavkeRacuna: StavkeRacuna[] = [];
    racunId: number | null = null;
    kolicina: number = 0;
    proizvodi: Proizvod[] = [];
    popust: number = 0;
    cart: { proizvod: Proizvod; kolicina: number; popust: number }[] = [];
    selectedProizvod: Proizvod | null = null;
    selectedProductId: number | null = null;
    selectedQuantity: number = 1;
    selectedDiscount: number = 0; 

    constructor(private http: HttpClient, private router: Router, private proizvodService: ProizvodService) { }

    ngOnInit() {
        this.loadProizvodi();
    }

    loadProizvodi(): void {
        this.proizvodService.getProizvodi().subscribe({
            next: (data) => {
                this.availableProizvodi = data;
                console.log('Loaded products:', this.availableProizvodi);  // Ensure Cena is here for each product
            },
            error: (err) => console.error('Error loading products', err)
        });
    }


    dodajUkorpu() {
        if (this.selectedProizvod && this.kolicina > 0) {
            const proizvodZaKorpu = {
                proizvod: this.selectedProizvod,  // Proizvod je sada postavljen ispravno
                kolicina: this.kolicina,
                popust: this.popust || 0,
            };
            this.cart.push(proizvodZaKorpu); // Dodavanje proizvoda u korpu
            console.log('Proizvod dodat u korpu:', proizvodZaKorpu);
            console.log('Trenutni sadržaj korpe:', this.cart);
            this.kolicina = 1; // Resetovanje količine nakon dodavanja
            this.popust = 0; // Resetovanje popusta nakon dodavanja
        } else {``
            console.error('Nema proizvoda za dodavanje u korpu ili količina je nevalidna.');
        }
    }
    
    
    


    // Handle product selection from dropdown
    onProductSelect(event: Event) {
        const selectElement = event.target as HTMLSelectElement;
        const selectedProductId = +selectElement.value; // ID izabranog proizvoda
        
        // Pronađite proizvod na osnovu ID-a
        const proizvod = this.availableProizvodi.find(p => p.ProizvodID === selectedProductId);
        
        if (proizvod) {
            this.selectedProizvod = proizvod;
            console.log('Izabran proizvod:', this.selectedProizvod);
        } else {
            console.error('Proizvod nije pronađen!');
            this.selectedProizvod = null; // Ako nije pronađen, postavite na null
        }
    }

    // Add selected product to cart
    addToRacun(proizvodId: number | null, quantity: number, discount: number) {
        if (proizvodId && quantity > 0) {
            const selectedProduct = this.availableProizvodi.find(p => p.ProizvodID === proizvodId);
            if (selectedProduct) {
                this.cart.push({
                    proizvod: selectedProduct,
                    kolicina: quantity,
                    popust: discount
                });
                // Reset selection
                this.selectedProductId = null;
                this.selectedQuantity = 1;
                this.selectedDiscount = 0;
                console.log('Product added to cart:', selectedProduct);
            }
        } else {
            console.error('Invalid product or quantity');
        }
    }


    onLogout() {
        localStorage.removeItem('token');
        this.router.navigateByUrl('/signup');
    }
}

