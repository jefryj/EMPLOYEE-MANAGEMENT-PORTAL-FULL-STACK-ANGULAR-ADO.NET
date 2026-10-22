import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { LoginDto } from '../models/login';
import { LoginResponse} from '../models/login-response';
import { ChangePasswordDto } from '../models/change-password';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  private apiUrl = 'http://localhost:5187/api/auth';

  constructor(private http: HttpClient) {
  }

  login(dto: LoginDto): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.apiUrl}/login`,
      dto
    );
  }

  changePassword(dto: ChangePasswordDto): Observable<string> {
    const token = localStorage.getItem('token');
    return this.http.post<string>(
      `${this.apiUrl}/change-password`,
      dto,
      {
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );
  }
}