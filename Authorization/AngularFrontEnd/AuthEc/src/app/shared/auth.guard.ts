import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router)


  if (authService.isLoggedIn()) {
    console.log('User is logged in');
    return true;
  } else {
    console.log('User is not logged in, redirecting to signin');
    router.navigateByUrl('/signin');
    return false;
  }
  
};
