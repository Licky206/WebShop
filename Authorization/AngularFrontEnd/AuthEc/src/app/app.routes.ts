import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login', 
    pathMatch: 'full'
  },
  {
    path: '',
    component: UserComponent,
    children: [
      { path: 'signup', component: RegistrationComponent },
      { path: 'login', component: LoginComponent } // Login route
    ]
  },
  { path: 'dashboard', component: DashboardComponent }
];