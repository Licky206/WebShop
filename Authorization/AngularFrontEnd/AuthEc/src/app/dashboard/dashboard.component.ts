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
    isSwitchOn: boolean = false; // Tracks switch state
    statusiRacuni = [ 'U izradi', 'FISKALIZIRAN', 'STORNIRAN' ];
    


    newStatus: string = '';  // New status for the invoice

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
      if (this.selectedProizvod && this.selectedProizvod.proizvodID) {
          const kolicinaString = this.kolicina.toString();
          const popustString = this.popust.toString();
  
          if (kolicinaString.length > 8) {
              console.error('Količina ne može imati više od 8 cifara.');
              alert('Količina ne može imati više od 8 cifara.');
              return;
          }
  
          if (popustString.length > 8) {
              console.error('Popust ne može imati više od 8 cifara.');
              alert('Popust ne može imati više od 8 cifara.');
              return;
          }
  
          // Vaša logika za dodavanje u korpu
          const existingItem = this.cart.find(
              (item) =>
                  item.proizvod.proizvodID === this.selectedProizvod?.proizvodID &&
                  item.popust === this.popust
          );
  
          if (existingItem) {
              existingItem.kolicina += this.kolicina;
          } else {
              this.cart.push({
                  proizvod: this.selectedProizvod,
                  kolicina: this.kolicina,
                  popust: this.popust || 0,
              });
          }
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

    deleteRacun(racunId: number): void {
        if (confirm('Da li ste sigurni da želite da obrišete račun?')) {
          this.racunService.deleteRacun(racunId).subscribe({
            next: () => {
              console.log(`Račun ${racunId} uspešno obrisan`);
              this.fetchRacuni(); // Osvetljavanje liste računa
            },
            error: (err) => {
              console.error('Greška pri brisanju računa:', err);
            }
          });
        }
      }

      updateRacunStatus(racunId: number, newStatus: string) {
        if (!newStatus) {
          alert('Please enter a valid status.');
          return;
        }
        const statusUpdateRequest = { newStatus: newStatus };

          console.log('Sending request:', statusUpdateRequest);  // Log to verify the payload

        // Call your service to update the status in the database
        this.racunService.updateRacunStatus(racunId, newStatus).subscribe({
          next: (response) => {
            if (response) {
              alert('Status updated successfully!');
              this.fetchRacuni();  // Reload the invoices list
              this.selectedRacunId = null;  // Reset selection
            } else {
              alert('Failed to update status');
            }
          },
          error: (err) => {
            console.error('Error updating status:', err);
            alert('An error occurred while updating the status.');
          }
        });
      }
      
    editRacun(racun: any, event: Event) {
        event.stopPropagation();
        this.selectedRacunId = racun.racunId;
        this.newStatus = racun.statusRacuna;  // Pre-fill the input with the current status
      }      
    

    //logika za prikaz racuna:
    fetchRacuni(): void {
        this.racunService.getAllRacuni().subscribe((data) => {
          this.racuni = data;
        });
      }
    
    fetchStavke(racunId: number): void {

      if (!this.isSwitchOn) {
        console.log("Switch je Off, tako da Stavke se nece prikazati");
        return;
      }


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

    onToggleSwitch(): void {
      if (this.isSwitchOn) {
        this.isStavkeModalVisible = false;
      }
  }
  
    showStavkeModel(racunId: number): void{
      if(!this.isSwitchOn)return;

      this.selectedRacunId = racunId;
      this.fetchStavke(racunId);
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

    //RacunStatus logic

    canEdit(status: string): boolean {
      return status === 'U izradi' || status === 'FISKALIZIRAN';
    }
    
    canDelete(status: string): boolean {
      return status === 'STORNIRAN';
    }
    
    canChangeToFiskaliziran(status: string): boolean {
      return status === 'U izradi';
    }
    
    canChangeToStorniran(status: string): boolean {
      return status === 'FISKALIZIRAN';
    }
    
    changeStatus( racunId: number, newStatus: string):void  {

      const racun = this.racuni.find(r => r.id === racunId);

      if (!racun) {
        console.error('Racun nije pronadjen');
        return
      }
        // Validacija promene statusa
      if (
        (newStatus === 'FISKALIZIRAN' && !this.canChangeToFiskaliziran(racun.statusRacuna)) ||
        (newStatus === 'STORNIRAN' && !this.canChangeToStorniran(racun.statusRacuna))
      ) {
        console.error('Nije dozvoljena promena statusa na ovaj način.');
        return;
      }


      racun.statusRacuna = newStatus;
      this.racunService.updateRacunStatus(racunId, newStatus).subscribe(
        () => {
          console.log(`Status računa ID: ${racunId} promenjen u ${newStatus}`);
        },
        (error) => {
          console.error('Greška prilikom promene statusa:', error);
        }
      );



    }



    canChangeStatus(currentStatus: string): string[] {
      switch (currentStatus) {
        case 'U izradi':
          return ['FISKALIZIRAN']; 
        case 'FISKALIZIRAN':
          return ['STORNIRAN'];
        case 'STORNIRAN':
          return [];
        default:
          return [];
      }
    }

    onStatusChange(racunId: number, newStatus: string): void {
      // Logika za promenu statusa
      const racun = this.racuni.find(r => r.id === racunId);
      if (racun && this.canChangeStatus(racun.statusRacuna).includes(newStatus)) {
        racun.statusRacuna = newStatus;
        this.racunService.updateRacunStatus(racunId, newStatus).subscribe(
          () => {
            console.log(`Status računa ID: ${racunId} promenjen u ${newStatus}`);
          },
          (error) => {
            console.error('Greška prilikom promene statusa:', error);
          }
        );
      } else {
        console.error('Nije dozvoljena promena statusa u odabrani status.');
      }
    }

    onLogout() {
        localStorage.removeItem('token');
        this.router.navigateByUrl('/signup');
    }
    closeStavkeModeal(){
        this.isStavkeModalVisible = false;
    }

    refreshPage(): void {
      window.location.reload();
        this.loadProizvodi(); // Osveži proizvode
        this.fetchRacuni();   // Osveži račune
        this.cart = []; 
    }

    onKolicinaInput(event: Event): void {
      const inputElement = event.target as HTMLInputElement;
  
      // Dozvoljava samo do 8 cifara
      if (inputElement.value.length > 8) {
          inputElement.value = inputElement.value.slice(0, 8);
      }
  
      // Ažurirajte vrednost modela (u slučaju da koristite [(ngModel)])
      this.kolicina = Number(inputElement.value);
  }

  get ukupnaCena(): number {
    return this.cart.reduce((total, item) => {
      const itemCena = item.proizvod.cena * item.kolicina * (1 - item.popust / 100);
      return total + itemCena;
    }, 0);
  }
  
  onPopustInput(event: Event): void {
    const inputElement = event.target as HTMLInputElement;

    // Ograničenje na maksimalnih 90
    if (Number(inputElement.value) > 90) {
        inputElement.value = '90';
    }

    // Ažurirajte vrednost modela (u slučaju da koristite [(ngModel)])
    this.popust = Number(inputElement.value);
}


 
 
}