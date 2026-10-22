import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  DepartmentCount,
  DepartmentSalarySummary,
  EmployeeSummary,
  ProjectCount,
  ProjectSalarySummary
} from '../models/report';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = 'http://localhost:5187/api/reports';

  constructor(private http: HttpClient) { }

  getEmployeeSummary(): Observable<EmployeeSummary> {
    return this.http.get<EmployeeSummary>(`${this.apiUrl}/employee-summary`, { headers: this.authHeaders() });
  }

  getDepartmentCount(): Observable<DepartmentCount[]> {
    return this.http.get<DepartmentCount[]>(`${this.apiUrl}/department-count`, { headers: this.authHeaders() });
  }

  getDepartmentSalarySummary(): Observable<DepartmentSalarySummary[]> {
    return this.http.get<DepartmentSalarySummary[]>(`${this.apiUrl}/department-salary-summary`, { headers: this.authHeaders() });
  }

  getProjectCount(): Observable<ProjectCount[]> {
    return this.http.get<ProjectCount[]>(`${this.apiUrl}/project-count`, { headers: this.authHeaders() });
  }

  getProjectSalarySummary(): Observable<ProjectSalarySummary[]> {
    return this.http.get<ProjectSalarySummary[]>(`${this.apiUrl}/project-salary-summary`, { headers: this.authHeaders() });
  }

  private authHeaders(): { Authorization: string } {
    return { Authorization: `Bearer ${localStorage.getItem('token')}` };
  }
}