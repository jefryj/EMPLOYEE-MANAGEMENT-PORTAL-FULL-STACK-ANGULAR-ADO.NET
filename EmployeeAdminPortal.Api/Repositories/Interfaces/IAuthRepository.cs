using EmployeeAdminPortal.Api.Models.Entities;

namespace EmployeeAdminPortal.Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Employee?> GetEmployeeByEmailAsync(string email);

    Task ChangePasswordAsync(int employeeId, string passwordHash);
}