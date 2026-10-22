using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Models.DTOs;

namespace EmployeeAdminPortal.Api.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeesAsync(EmployeeSearchDto searchDto);

    Task<Employee?> GetByIdAsync(int id);

    Task<bool> AnyByEmailAsync(string email);

    Task AddAsync(Employee employee);

    Task UpdateAsync(Employee employee);

    Task DeleteAsync(int id);
}