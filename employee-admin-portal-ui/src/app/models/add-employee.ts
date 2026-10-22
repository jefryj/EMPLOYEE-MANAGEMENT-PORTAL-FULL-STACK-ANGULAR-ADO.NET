export interface AddEmployeeDto {
  name: string;
  email: string;
  phone: string;
  salary: number;
  password: string;
  departmentId: number;
  projectId?: number;
  roleId: number;
}