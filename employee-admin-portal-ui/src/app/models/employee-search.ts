export interface EmployeeSearch {
  name?: string;
  email?: string;
  departmentId?: number | null;
  projectId?: number | null;
  sortBy?: string;
  isDescending?: boolean;
  pageNumber?: number;
  pageSize?: number;
}