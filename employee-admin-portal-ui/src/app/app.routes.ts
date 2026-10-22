import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Home } from './pages/home/home';
import { authGuard } from './guards/auth-guard';
import { Employees } from './pages/employees/employees';
import { AddEmployee } from './pages/add-employee/add-employee';
import { adminGuard } from './guards/admin-guard';
import { UpdateEmployee } from './pages/update-employee/update-employee';
import { MyProfile } from './pages/my-profile/my-profile';
import { Departments } from './pages/departments/departments';
import { Projects } from './pages/projects/projects';
import { Reports } from './pages/reports/reports';
import { AuditLogs } from './pages/audit-logs/audit-logs';
import { Dashboard } from './pages/dashboard/dashboard';
import { ChangePassword } from './pages/change-password/change-password';

export const routes: Routes = [
  {
    path: '',
    component: Login
  },
  {
    path: 'home',
    component: Home,
    canActivate: [authGuard]
  },
  {
    path: 'employees',
    component: Employees,
    canActivate: [authGuard]
  },
  {
    path: 'add-employee',
    component: AddEmployee,
    canActivate: [authGuard, adminGuard]
  },
  {
    path: 'update-employee/:id',
    component: UpdateEmployee,
    canActivate: [authGuard, adminGuard]
  },
  {
  path: 'my-profile',
  component: MyProfile,
  canActivate: [authGuard]
  },
  {
    path: 'departments',
    component: Departments,
    canActivate: [authGuard]
  },
  {
    path: 'projects',
    component: Projects,
    canActivate: [authGuard]
  },
  {
    path: 'reports',
    component: Reports,
    canActivate: [authGuard, adminGuard]
  },
  {
    path: 'audit-logs',
    component: AuditLogs,
    canActivate: [authGuard, adminGuard]
  },
  {
    path: 'dashboard',
    component: Dashboard,
    canActivate: [authGuard, adminGuard]
  },
  {
    path: 'change-password',
    component: ChangePassword,
    canActivate: [authGuard]
  }
];