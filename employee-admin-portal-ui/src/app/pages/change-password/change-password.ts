import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { ChangePasswordDto } from '../../models/change-password';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css'
})
export class ChangePassword {
  currentPassword = '';
  newPassword = '';
  confirmNewPassword = '';
  successMessage = '';
  errorMessage = '';

  constructor(private authService: AuthService) {}

  changePassword(): void {
    const dto: ChangePasswordDto = {
      employeeId: Number(localStorage.getItem('employeeId')),
      currentPassword: this.currentPassword,
      newPassword: this.newPassword
    };

    this.successMessage = '';
    this.errorMessage = '';

    this.authService.changePassword(dto).subscribe({
      next: () => {
        this.successMessage = 'Password changed successfully.';

        this.currentPassword = '';
        this.newPassword = '';
        this.confirmNewPassword = '';
      },
      error: (error) => {
        console.error(error);

        this.errorMessage = error.error?.title ?? 'Unable to change password.';
      }
    });
  }
}