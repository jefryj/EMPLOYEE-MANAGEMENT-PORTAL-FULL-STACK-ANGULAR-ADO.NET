export interface Employee {
  id: number;
  name: string;
  email: string;
  phone: string;
  salary: number;
  departmentId: number;
  departmentName: string;
  projectId?: number;
  projectName?: string;
  roleId: number;
  roleName: string;
}