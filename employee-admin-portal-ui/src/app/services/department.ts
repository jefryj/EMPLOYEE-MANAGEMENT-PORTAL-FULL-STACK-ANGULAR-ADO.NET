import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Department, DepartmentDto } from '../models/department';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private apiUrl = 'http://localhost:5187/api/departments';

  constructor(private http: HttpClient) { }

  getDepartments(): Observable<Department[]> {
    const token = localStorage.getItem('token');

    return this.http.get<Department[]>(this.apiUrl, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  addDepartment(department: DepartmentDto): Observable<string> {
    const token = localStorage.getItem('token');

    return this.http.post<string>(this.apiUrl, department, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  updateDepartment(id: number, department: DepartmentDto): Observable<string> {
    const token = localStorage.getItem('token');

    return this.http.put<string>(`${this.apiUrl}/${id}`, department, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  deleteDepartment(id: number): Observable<string> {
    const token = localStorage.getItem('token');

    return this.http.delete<string>(`${this.apiUrl}/${id}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
}