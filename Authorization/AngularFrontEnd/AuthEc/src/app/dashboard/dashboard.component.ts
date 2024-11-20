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
    racunId: number | null = null;   
    kolicina: number = 0;
    proizvodi: Proizvod[] = [];
    popust: number = 0;
    cart: { proizvod: Proizvod; kolicina: number; popust: number }[] = [];
    selectedProizvod: Proizvod | null = null;
    selectedQuantity: number = 1;
    selectedDiscount: number = 0;
    racuni: any[] = [];
    stavke: any[] = [];
    selectedRacunId: number | null = null;
    isStavkeModalVisible: boolean = false;


    constructor(private http: HttpClient, private router: Router, private proizvodService: ProizvodService, private racunService: RacunService, private stavkeRacunaService: StavkeRacunaService) { }

    ngOnInit() {
        this.loadProizvodi();
        this.fetchRacuni();
    }

    loadProizvodi() {
    
        this.proizvodService.getProizvodi().subscribe(
          (data) => {
            this.proizvodi = data;
            console.log('Učitali proizvode:', this.proizvodi); 
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
        console.log('Selected Product:', this.selectedProizvod);  
        if (this.selectedProizvod && this.selectedProizvod.proizvodID) { 
            const proizvodZaKorpu = {
                proizvod: this.selectedProizvod,
                kolicina: this.kolicina,
                popust: this.popust || 0,
            };
    
            this.cart.push(proizvodZaKorpu);  
            console.log('Product added to cart:', proizvodZaKorpu);
            console.log('Current cart contents:', this.cart);
    
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
                return {
                    racunId: 0, 
                    proizvodID: item.proizvod.proizvodID, 
                    kolicina: item.kolicina,  
                    popust: item.popust  
                };
            })
        };
    
        console.log('Racun Request:', JSON.stringify(racunRequest));
    
        // Pozivanje servisa 
        this.racunService.kreirajRacunSaStavkama(racunRequest).subscribe({
            next: (response) => {
                console.log('Invoice created successfully:', response);
                this.cart = []; 
            },
            error: (err) => {
                console.error('Error creating invoice:', err);
            }
        });
    }



    //logika za prikaz racuna:
    fetchRacuni(): void {
        this.racunService.getAllRacuni().subscribe((data) => {
          this.racuni = data;
        });
      }
    
      fetchStavke(racunId: number): void {
        this.selectedRacunId = racunId;
        this.racunService.getStavkeByRacunId(racunId).subscribe((data) => {
            this.stavke = data.map(stavka => {
                const proizvod = this.proizvodi.find(p => p.proizvodID === stavka.proizvodID);
                return {
                    ...stavka,
                    nazivProizvoda: proizvod ? proizvod.nazivProizvoda : 'Nepoznat proizvod'
                };
            });
            console.log('Fetched StavkeRacuna with product names for RacunId:', racunId);
            this.isStavkeModalVisible = true;
        });
    }
    




    // In DashboardComponent
    deleteProductFromCart(proizvodId: number) {
        const index = this.cart.findIndex(item => item.proizvod.proizvodID === proizvodId);
        if (index !== -1) {
            this.cart.splice(index, 1);
            console.log(`Proizvod sa ID ${proizvodId} obrisan iz korpe`);
        }
    }

    updateProductInCart(proizvodId: number, kolicina: number, popust: number) {
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


    closeStavkeModeal(){
        this.isStavkeModalVisible = false;
    }
}