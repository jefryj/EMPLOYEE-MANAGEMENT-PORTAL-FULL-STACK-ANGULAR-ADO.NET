import { Component, OnInit } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { DepartmentCount, DepartmentSalarySummary, EmployeeSummary, ProjectCount, ProjectSalarySummary } from '../../models/report';
import { ReportService } from '../../services/report';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './reports.html',
  styleUrl: './reports.css'
})
export class Reports implements OnInit {
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
      next: (result) => this.departmentCounts = result,
      error: (error) => this.handleError(error)
    });
    this.reportService.getDepartmentSalarySummary().subscribe({
      next: (result) => this.departmentSalarySummaries = result,
      error: (error) => this.handleError(error)
    });
    this.reportService.getProjectCount().subscribe({
      next: (result) => this.projectCounts = result,
      error: (error) => this.handleError(error)
    });
    this.reportService.getProjectSalarySummary().subscribe({
      next: (result) => this.projectSalarySummaries = result,
      error: (error) => this.handleError(error)
    });
  }

  private handleError(error: unknown): void {
    console.error(error);
    this.errorMessage = 'Unable to load one or more reports.';
  }
}