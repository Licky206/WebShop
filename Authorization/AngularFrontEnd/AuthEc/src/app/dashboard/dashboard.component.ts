import { Component, input, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Proizvod, StavkeRacuna } from '../../models/models';
import { ProizvodService } from '../shared/services/proizvod/services/proizvod.service';
import { RacunService } from '../shared/services/racun.service';
import { StavkeRacunaService } from '../shared/stavkeRacuna.service';

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
    racunId: number | null = null;  // RacunID je broj ili null
    kolicina: number = 0;
    proizvodi: Proizvod[] = [];
    popust: number = 0;
    cart: { proizvod: Proizvod; kolicina: number; popust: number }[] = [];
    selectedProizvod: Proizvod | null = null;
    selectedProductId: number | null = null;
    selectedQuantity: number = 1;
    selectedDiscount: number = 0;

    constructor(private http: HttpClient, private router: Router, private proizvodService: ProizvodService,private racunService: RacunService ,  private stavkeRacunaService: StavkeRacunaService ) { }

    ngOnInit() {
        this.loadProizvodi();
    }

    loadProizvodi(): void {
        this.proizvodService.getProizvodi().subscribe({
            next: (data) => {
                this.availableProizvodi = data;
                console.log('Loaded products:', this.availableProizvodi);
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

    createRacun(): void {
        this.racunService.createRacun().subscribe({
          next: (response) => {
            console.log('Račun uspešno kreiran', response);
            // You can assign the returned invoice ID to `racunId` if necessary
            this.racunId = response.racunId; // Assuming the backend returns an ID
          },
          error: (err) => {
            console.error('Greška prilikom kreiranja računa:', err);
          }
        });
      }

      finalizeRacun() {

        if (this.racunId === null) {
            console.error('Račun nije kreiran');
            return;
        }
    
        // Mapirajte stavke sa pretvorbom RacunID u broj
        const stavke: StavkeRacuna[] = this.cart.map(item => ({
            ProizvodID: item.proizvod.ProizvodID,
            Kolicina: item.kolicina,
            Popust: item.popust,
            proizvod: item.proizvod, // Dodajte proizvod
            RacunID: this.racunId as number // Pretvaramo null u number
        }));
        console.log("Podaci koji se šalju na server:", stavke);

        this.stavkeRacunaService.addStavkeRacuna(this.racunId, stavke).subscribe({
            next: () => {
                console.log('Stavke uspešno dodate u račun');
                this.cart = [];
                this.racunId = null;
            },
            error: (err) => {
                console.error('Greška prilikom dodavanja stavki u račun:', err);
            }
        });
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
    // In DashboardComponent
    deleteProductFromCart(proizvodId: number) {
        // Brisanje proizvoda iz korpe
        const index = this.cart.findIndex(item => item.proizvod.ProizvodID === proizvodId);
        if (index !== -1) {
            this.cart.splice(index, 1);
            console.log(`Proizvod sa ID ${proizvodId} obrisan iz korpe`);
        }
    }
    // In DashboardComponent
    updateProductInCart(proizvodId: number, kolicina: number, popust: number) {
        // Pronađi proizvod u korpi
        const productIndex = this.cart.findIndex(item => item.proizvod.ProizvodID === proizvodId);
        if (productIndex !== -1) {
            this.cart[productIndex].kolicina = kolicina;
            this.cart[productIndex].popust = popust;

            console.log(`Proizvod sa ID ${proizvodId} ažuriran u korpi`);
        } else {
            console.error("Proizvod nije pronađen u korpi.");
        }
    }




    onLogout() {
        localStorage.removeItem('token');
        this.router.navigateByUrl('/signup');
    }
}

