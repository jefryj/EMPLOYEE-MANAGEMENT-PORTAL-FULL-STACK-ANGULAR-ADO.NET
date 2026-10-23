import { Component, OnInit } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { DepartmentCount, DepartmentSalarySummary, EmployeeSummary, ProjectCount, ProjectSalarySummary } from '../../models/report';
import { ReportService } from '../../services/report';
import { Chart } from 'chart.js/auto';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './reports.html',
  styleUrl: './reports.css'
})
export class Reports implements OnInit {

  roleId = Number(localStorage.getItem('roleId'));
  departmentChart!: Chart;
  projectChart!: Chart;

  employeeSummary: EmployeeSummary | null = null;
  departmentCounts: DepartmentCount[] = [];
  departmentSalarySummaries: DepartmentSalarySummary[] = [];
  projectCounts: ProjectCount[] = [];
  projectSalarySummaries: ProjectSalarySummary[] = [];
  errorMessage = '';

  constructor(private reportService: ReportService) { }

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.reportService.getEmployeeSummary().subscribe({
      next: (result) => this.employeeSummary = result,
      error: (error) => this.handleError(error)
    });
    
    this.reportService.getDepartmentCount().subscribe({
    next: (result) => {
      this.departmentCounts = result;
      this.createDepartmentChart();
    },
    error: (error) => this.handleError(error)
  });

    this.reportService.getDepartmentSalarySummary().subscribe({
      next: (result) => this.departmentSalarySummaries = result,
      error: (error) => this.handleError(error)
    });
    
    
    this.reportService.getProjectCount().subscribe({
      next: (result) => {
        this.projectCounts = result;

        setTimeout(() => {
          this.createProjectChart();
        });
      },
      error: (error) => this.handleError(error)
    });

    this.reportService.getProjectSalarySummary().subscribe({
      next: (result) => this.projectSalarySummaries = result,
      error: (error) => this.handleError(error)
    });
  }

  private createDepartmentChart(): void {

    this.departmentChart = new Chart('departmentChart', {
      type: 'doughnut',
      data: {
        labels: this.departmentCounts.map(d => d.departmentName),
        datasets: [{
          data: this.departmentCounts.map(d => d.employeeCount),
          backgroundColor: [
            '#36A2EB',
            '#FF6384',
            '#4BC0C0',
            '#FFCE56',
            '#9966FF',
            '#FF9F40'
          ]
        }]
      }
    });

  }

  private createProjectChart(): void {

    this.projectChart = new Chart('projectChart', {
      type: 'doughnut',
      data: {
        labels: this.projectCounts.map(p => p.projectName),
        datasets: [{
          data: this.projectCounts.map(p => p.employeeCount),
          backgroundColor: [
            '#36A2EB',
            '#FF6384',
            '#4BC0C0',
            '#FFCE56',
            '#9966FF',
            '#FF9F40'
          ]
        }]
      }
    });

  }

  private handleError(error: unknown): void {
    console.error(error);
    this.errorMessage = 'Unable to load one or more reports.';
  }
}