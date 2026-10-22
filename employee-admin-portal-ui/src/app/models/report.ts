export interface EmployeeSummary {
  totalEmployees: number;
  averageSalary: number;
  minSalary: number;
  maxSalary: number;
}

export interface DepartmentCount {
  departmentId: number;
  departmentName: string;
  employeeCount: number;
}

export interface DepartmentSalarySummary extends DepartmentCount {
  averageSalary: number;
  minSalary: number;
  maxSalary: number;
}

export interface ProjectCount {
  projectId: number;
  projectName: string;
  employeeCount: number;
}

export interface ProjectSalarySummary extends ProjectCount {
  averageSalary: number;
  minSalary: number;
  maxSalary: number;
}