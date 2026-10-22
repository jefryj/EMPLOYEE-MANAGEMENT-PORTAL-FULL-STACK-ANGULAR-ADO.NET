export interface AuditLog {
  id: number;
  employeeId: number | null;
  userName: string;
  action: string;
  entityName: string;
  details: string | null;
  createdAt: string;
}