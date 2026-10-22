import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuditLog } from '../models/audit-log';

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
  private apiUrl = 'http://localhost:5187/api/auditlogs';

  constructor(private http: HttpClient) { }

  getAuditLogs(): Observable<AuditLog[]> {
    const token = localStorage.getItem('token');

    return this.http.get<AuditLog[]>(this.apiUrl, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
}