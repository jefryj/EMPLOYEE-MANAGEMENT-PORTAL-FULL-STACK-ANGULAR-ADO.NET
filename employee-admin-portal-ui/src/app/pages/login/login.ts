import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { LoginDto } from '../../models/login';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  email = '';
  password = '';

  private authService: AuthService;
  private router: Router;

  constructor(authService: AuthService, router: Router) {
      this.authService = authService;
      this.router = router;
  }

  login(): void {

    const dto: LoginDto = {
      email: this.email,
      password: this.password
    };

    this.authService.login(dto)
      .subscribe({
        next: (response) => {
          console.log(response);
          localStorage.setItem('token', response.token);
          localStorage.setItem('roleId', response.roleId.toString());
          localStorage.setItem('employeeId', response.employeeId.toString());
          this.router.navigate(['/home']);
        },
        error: (error) => {
          console.error(error);
          alert('Login failed. Please check your credentials and try again.');
        }
      });
  }
}