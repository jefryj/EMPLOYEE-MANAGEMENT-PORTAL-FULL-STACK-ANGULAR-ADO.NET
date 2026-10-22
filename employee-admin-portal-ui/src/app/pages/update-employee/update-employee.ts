import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../../services/employee';
import { DepartmentService } from '../../services/department';
import { ProjectService } from '../../services/project';
import { UpdateEmployeeDto } from '../../models/update-employee';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-employee',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './update-employee.html',
  styleUrl: './update-employee.css'
})
export class UpdateEmployee implements OnInit {

  employee: UpdateEmployeeDto = {
    name: '',
    email: '',
    phone: '',
    salary: 0,
    departmentId: 0,
    projectId: undefined
  };

  departments: any[] = [];
  projects: any[] = [];
  id = 0;

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private projectService: ProjectService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.departmentService.getDepartments().subscribe(response => {
      this.departments = response;
    });

    this.projectService.getProjects().subscribe(response => {
      this.projects = response;
    });


    this.employeeService.getEmployeeById(this.id).subscribe(response => {
      this.employee = {
          name: response.name,
          email: response.email,
          phone: response.phone,
          salary: response.salary,
          departmentId: response.departmentId,
          projectId: response.projectId
        };
      });
  }

  updateEmployee(): void {
    this.employeeService.updateEmployee(this.id, this.employee).subscribe(() => {
        this.router.navigate(['/employees']);
      });
  }
}