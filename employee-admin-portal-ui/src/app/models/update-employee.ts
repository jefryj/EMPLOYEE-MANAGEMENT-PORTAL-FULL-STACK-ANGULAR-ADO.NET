export interface UpdateEmployeeDto {
  name: string;
  email: string;
  phone: string;
  salary: number;
  departmentId: number;
  projectId?: number;
}