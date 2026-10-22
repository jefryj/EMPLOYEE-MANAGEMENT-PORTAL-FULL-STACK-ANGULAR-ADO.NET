import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../services/department';
import { ProjectService } from '../../services/project';
import { ReportService } from '../../services/report';
import { AuditLogService } from '../../services/audit-log';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  totalEmployees: number | null = null;
  totalDepartments: number | null = null;
  totalProjects: number | null = null;
  totalAuditLogs: number | null = null;
  errorMessage = '';

  constructor(
    private reportService: ReportService,
    private departmentService: DepartmentService,
    private projectService: ProjectService,
    private auditLogService: AuditLogService
  ) { }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.reportService.getEmployeeSummary().subscribe({
      next: (result) => this.totalEmployees = result.totalEmployees,
      error: (error) => this.handleError(error)
    });
    this.departmentService.getDepartments().subscribe({
      next: (result) => this.totalDepartments = result.length,
      error: (error) => this.handleError(error)
    });
    this.projectService.getProjects().subscribe({
      next: (result) => this.totalProjects = result.length,
      error: (error) => this.handleError(error)
    });
    this.auditLogService.getAuditLogs().subscribe({
      next: (result) => this.totalAuditLogs = result.length,
      error: (error) => this.handleError(error)
    });
  }

  private handleError(error: unknown): void {
    console.error(error);
    this.errorMessage = 'Unable to load one or more dashboard totals.';
  }
}