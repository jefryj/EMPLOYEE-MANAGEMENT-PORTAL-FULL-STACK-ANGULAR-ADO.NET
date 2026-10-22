import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Department, DepartmentDto } from '../../models/department';
import { DepartmentService } from '../../services/department';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './departments.html',
  styleUrl: './departments.css'
})
export class Departments implements OnInit {
  departments: Department[] = [];
  roleId = Number(localStorage.getItem('roleId'));
  departmentName = '';
  editingId: number | null = null;
  errorMessage = '';

  constructor(private departmentService: DepartmentService) { }

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: (response) => {
        this.departments = response;
        this.errorMessage = '';
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to load departments.';
      }
    });
  }

  saveDepartment(): void {
    if (this.roleId !== 1 || !this.departmentName.trim()) {
      return;
    }

    const department: DepartmentDto = { departmentName: this.departmentName.trim() };
    const request = this.editingId === null
      ? this.departmentService.addDepartment(department) : this.departmentService.updateDepartment(this.editingId, department);

    request.subscribe({
      next: () => {
        this.departmentName = '';
        this.editingId = null;
        this.loadDepartments();
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to save department.';
      }
    });
  }

  editDepartment(department: Department): void {
    if (this.roleId !== 1) {
      return;
    }

    this.editingId = department.id;
    this.departmentName = department.departmentName;
  }

  cancelEdit(): void {
    this.editingId = null;
    this.departmentName = '';
  }

  deleteDepartment(id: number): void {
    if (this.roleId !== 1 || !confirm('Are you sure you want to delete this department?')) {
      return;
    }

    this.departmentService.deleteDepartment(id).subscribe({
      next: () => this.loadDepartments(),
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to delete department.';
      }
    });
  }
}