import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../services/employee';
import { Employee } from '../../models/employee';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Department } from '../../models/department';
import { DepartmentService } from '../../services/department';
import { Project } from '../../models/project';
import { ProjectService } from '../../services/project';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './employees.html',
  styleUrl: './employees.css'
})
export class Employees implements OnInit {
  searchId = 0;

  employees: Employee[] = [];
  departments: Department[] = [];
  projects: Project[] = [];
  roleId = Number(localStorage.getItem('roleId'));
  searchName = '';
  searchEmail = '';
  projectId: number | null = null;
  departmentId: number | null = null;
  sortBy = '';
  isDescending = false;
  currentPage = 1;
  readonly pageSize = 10;
  hasNextPage = false;
  private readonly employeeService: EmployeeService;  

  constructor(
    employeeService: EmployeeService,
    private router: Router,
    private departmentService: DepartmentService,
    private projectService: ProjectService
  ) {
    this.employeeService = employeeService;
  }

  ngOnInit(): void {
    this.loadEmployees();
    this.departmentService.getDepartments().subscribe({
      next: (response) => this.departments = response,
      error: (error) => console.error(error)
    });
    this.projectService.getProjects().subscribe({
      next: (response) => this.projects = response,
      error: (error) => console.error(error)
    });
  }

  loadEmployees(reloadedAfterEmptyPage = false): void {this.employeeService.getEmployees({
      name: this.searchName.trim() || undefined,
      email: this.searchEmail.trim() || undefined,
      departmentId: this.departmentId,
      projectId: this.projectId,
      sortBy: this.sortBy || undefined,
      isDescending: this.isDescending,
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    })
    .subscribe({
        next: (response) => {
          if (this.currentPage > 1 && response.length === 0) {
            this.currentPage--;
            this.loadEmployees(true);
            return;
          }
          this.employees = response;
          this.hasNextPage = response.length === this.pageSize && !reloadedAfterEmptyPage;
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

  applyFilters(): void {
    this.currentPage = 1;
    this.searchId = 0;
    this.loadEmployees();
  }

  previousPage(): void {
    if (this.currentPage <= 1) {
      return;
    }

    this.currentPage--;
    this.loadEmployees();
  }

  nextPage(): void {
    if (!this.hasNextPage) {
      return;
    }

    this.currentPage++;
    this.loadEmployees();
  }

  clearFilters(): void {
    this.searchName = '';
    this.searchEmail = '';
    this.departmentId = null;
    this.projectId = null;
    this.sortBy = '';
    this.isDescending = false;
    this.searchId = 0;
    this.currentPage = 1;
    this.loadEmployees();
  }
  updateEmployee(id: number): void {
  this.router.navigate(['/update-employee', id]);
}
searchEmployee(): void {

  if (!this.searchId) {
    return;
  }

  this.currentPage = 1;

  this.employeeService.getEmployeeById(this.searchId).subscribe({
      next: (response) => {
        this.employees = [response];
        this.hasNextPage = false;
      },
      error: (error) => {
        console.error(error);
        alert('Employee not found');
      }
    });
}

deleteEmployee(id: number): void {
  if (this.roleId !== 1 || !confirm('Are you sure you want to delete this employee?')) {
    return;
  }

  this.employeeService.deleteEmployee(id).subscribe({
      next: () => this.loadEmployees(),
      error: (error) => {
        console.error(error);
        alert('Unable to delete employee.');
      }
    });
}
}