import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../models/employee';
import { AddEmployeeDto } from '../models/add-employee';
import { UpdateEmployeeDto } from '../models/update-employee';
import { EmployeeSearch } from '../models/employee-search';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private apiUrl = 'http://localhost:5187/api/employees';

  constructor(private http: HttpClient) { }

  getEmployees(search: EmployeeSearch = {}): Observable<Employee[]> {

  const token = localStorage.getItem('token');
  let params = new HttpParams();

  if (search.name) {
    params = params.set('Name', search.name);
  }
  if (search.email) {
    params = params.set('Email', search.email);
  }
  if (search.departmentId !== undefined && search.departmentId !== null) {
    params = params.set('DepartmentId', search.departmentId);
  }
  if (search.projectId !== undefined && search.projectId !== null) {
  params = params.set('ProjectId', search.projectId);
}
  if (search.sortBy) {
    params = params.set('SortBy', search.sortBy);
    params = params.set('IsDescending', search.isDescending ?? false);
  }
  params = params.set('PageNumber', search.pageNumber ?? 1);
  params = params.set('PageSize', search.pageSize ?? 10);

  return this.http.get<Employee[]>(
    this.apiUrl,
    {
      params,
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  );
}
addEmployee(employee: AddEmployeeDto) {
  const token = localStorage.getItem('token');

  return this.http.post(
    this.apiUrl,
    employee,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  );
}
getEmployeeById(id: number): Observable<Employee> {

  const token = localStorage.getItem('token');

  return this.http.get<Employee>(
    `${this.apiUrl}/${id}`,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  );
}

updateEmployee(id: number, employee: UpdateEmployeeDto) {

  const token = localStorage.getItem('token');

  return this.http.put(
    `${this.apiUrl}/${id}`,
    employee,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  );
}

deleteEmployee(id: number): Observable<string> {
  const token = localStorage.getItem('token');

  return this.http.delete<string>(
    `${this.apiUrl}/${id}`,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  );
}
}