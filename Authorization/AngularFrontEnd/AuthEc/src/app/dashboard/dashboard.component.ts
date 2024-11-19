import { Component, input, numberAttribute, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Proizvod, RacunRequest, StavkeRacuna } from '../../models/models';
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
    stavkeRacuna: StavkeRacuna[] = [];
    racunId: number | null = null;  // RacunID je broj ili null
    kolicina: number = 0;
    proizvodi: Proizvod[] = [];
    popust: number = 0;
    cart: { proizvod: Proizvod; kolicina: number; popust: number }[] = [];
    selectedProizvod: Proizvod | null = null;
    selectedQuantity: number = 1;
    selectedDiscount: number = 0;

    constructor(private http: HttpClient, private router: Router, private proizvodService: ProizvodService, private racunService: RacunService, private stavkeRacunaService: StavkeRacunaService) { }

    ngOnInit() {
        this.loadProizvodi();
    }

    loadProizvodi() {
        // Učitaj proizvode putem servisa (API poziv)
        this.proizvodService.getProizvodi().subscribe(
          (data) => {
            this.proizvodi = data;
            console.log('Učitali proizvode:', this.proizvodi);  // Debug: Proverite podatke
            // Uverite se da svaki proizvod ima ProizvodID
            this.proizvodi.forEach(proizvod => {
              if (!proizvod.proizvodID) {
                console.error('Proizvod bez validnog ProizvodID:', proizvod);
              }
            });
          },
          (error) => {
            console.error('Greška pri učitavanju proizvoda:', error);
          }
        );
    }
    

      DodajUkorpu() {
        console.log('Selected Product:', this.selectedProizvod);  // Debug
        if (this.selectedProizvod && this.selectedProizvod.proizvodID) {  // Use 'ProizvodID' (uppercase 'P')
            const proizvodZaKorpu = {
                proizvod: this.selectedProizvod,
                kolicina: this.kolicina,
                popust: this.popust || 0,
            };
    
            this.cart.push(proizvodZaKorpu);  // Add product to cart
            console.log('Product added to cart:', proizvodZaKorpu);
            console.log('Current cart contents:', this.cart);
    
            // Reset quantity and discount
            this.kolicina = 1;
            this.popust = 0;
        } else {
            console.error('Invalid ProductID for the selected product');
        }
    }
    
    
    kreirajRacunSaStavkama() {
        if (this.cart.length === 0) {
            console.error('Korpa je prazna, ne mogu da kreiram račun');
            return;
        }
    
        const racunRequest = {
            racun: [
                {
                    statusRacuna: 'U izradi',
                    datum: new Date().toISOString()
                }
            ],
            stavke: this.cart.map(item => {
                // Proveri da li ProizvodID postoji, ako ne postavi na 0 ili null
                if (!item.proizvod.proizvodID) {
                    console.error('ProizvodID is missing or invalid:', item);
                    item.proizvod.proizvodID = 0;  // Postavi na 0 ako nije validno
                }
                return {
                    racunId: this.racunId || 0,  // Ako racunId nije postavljen, postavi 0
                    proizvodID: item.proizvod.proizvodID, // Pravilno postavljanje ProizvodID
                    kolicina: item.kolicina,  // Preuzmi količinu iz stavke
                    popust: item.popust  // Preuzmi popust iz stavke
                };
            })
        };
    
        // Proveri kako izgleda request pre slanja
        console.log('Racun Request:', JSON.stringify(racunRequest));
    
        // Pozivanje servisa za kreiranje računa sa stavkama
        this.racunService.kreirajRacunSaStavkama(racunRequest).subscribe({
            next: (response) => {
                console.log('Invoice created successfully:', response);
                this.cart = []; // Isprazni korpu nakon uspešnog kreiranja računa
            },
            error: (err) => {
                console.error('Error creating invoice:', err);
            }
        });
    }
    
    



    

    // In DashboardComponent
    deleteProductFromCart(proizvodId: number) {
        // Brisanje proizvoda iz korpe
        const index = this.cart.findIndex(item => item.proizvod.proizvodID === proizvodId);
        if (index !== -1) {
            this.cart.splice(index, 1);
            console.log(`Proizvod sa ID ${proizvodId} obrisan iz korpe`);
        }
    }
    // In DashboardComponent
    updateProductInCart(proizvodId: number, kolicina: number, popust: number) {
        // Pronađi proizvod u korpi
        const productIndex = this.cart.findIndex(item => item.proizvod.proizvodID === proizvodId);
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