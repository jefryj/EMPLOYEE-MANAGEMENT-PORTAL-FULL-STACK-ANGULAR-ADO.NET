import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AddEmployeeDto } from '../../models/add-employee';
import { EmployeeService } from '../../services/employee';
import { CommonModule } from '@angular/common';
import { DepartmentService } from '../../services/department';
import { Department } from '../../models/department';
import { ProjectService } from '../../services/project';
import { Project } from '../../models/project';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-employee.html',
  styleUrl: './add-employee.css',
})
export class AddEmployee {

  constructor(
    private readonly employeeService: EmployeeService,
    private readonly departmentService: DepartmentService,
    private readonly projectService: ProjectService
  ) {
  }

  departments: Department[] = [];
  projects: Project[] = [];
  employee: AddEmployeeDto = {
    name: '',
    email: '',
    phone: '',
    salary: 0,
    password: '',
    departmentId: 0,
    projectId: undefined,
    roleId: 0
  };

  ngOnInit(): void {
    this.departmentService.getDepartments()
      .subscribe({
        next: (response) => {
          this.departments = response;
        },
        error: (error) => {
          console.error('Failed to load departments.', error);
        }
      });

    this.projectService.getProjects()
      .subscribe({
        next: (response) => {
          this.projects = response;
        },
        error: (error) => {
          console.error('Failed to load projects.', error);
        }
      });
  }

  addEmployee(): void {
    this.employeeService.addEmployee(this.employee)
      .subscribe({
        next: (response) => {
          alert('Employee added successfully.');
          console.log(response);
        },
        error: (error) => {
          alert('Failed to add employee.');
          console.error(error);
        }
      });
  }
}