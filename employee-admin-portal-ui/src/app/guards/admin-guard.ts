import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const adminGuard: CanActivateFn = () => {

  const router = inject(Router);

  const roleId = Number(localStorage.getItem('roleId'));

  if (roleId === 1) {
    return true;
  }

  return router.createUrlTree(['/home']);
};