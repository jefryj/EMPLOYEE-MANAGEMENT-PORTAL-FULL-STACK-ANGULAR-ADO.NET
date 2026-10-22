import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../services/employee';
import { Employee } from '../../models/employee';

@Component({
  selector: 'app-my-profile',
  standalone: true,
  imports: [],
  templateUrl: './my-profile.html',
  styleUrl: './my-profile.css'
})
export class MyProfile implements OnInit {

  employee?: Employee;

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {

    const employeeId = Number(
      localStorage.getItem('employeeId')
    );

    this.employeeService.getEmployeeById(employeeId).subscribe({
        next: (response) => {
          this.employee = response;
        },
        error: (error) => {
          console.error(error);
        }
      });
  }
}
