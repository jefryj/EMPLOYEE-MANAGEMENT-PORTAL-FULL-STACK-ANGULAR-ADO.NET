export interface ChangePasswordDto {
  employeeId: number;
  currentPassword: string;
  newPassword: string;
}