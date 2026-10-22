import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AuditLog } from '../../models/audit-log';
import { AuditLogService } from '../../services/audit-log';

@Component({
  selector: 'app-audit-logs',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './audit-logs.html',
  styleUrl: './audit-logs.css'
})
export class AuditLogs implements OnInit {
  auditLogs: AuditLog[] = [];
  errorMessage = '';

  constructor(private auditLogService: AuditLogService) { }

  ngOnInit(): void {
    this.loadAuditLogs();
  }

  loadAuditLogs(): void {
    this.auditLogService.getAuditLogs().subscribe({
      next: (response) => {
        this.auditLogs = response;
        this.errorMessage = '';
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to load audit logs.';
      }
    });
  }
}