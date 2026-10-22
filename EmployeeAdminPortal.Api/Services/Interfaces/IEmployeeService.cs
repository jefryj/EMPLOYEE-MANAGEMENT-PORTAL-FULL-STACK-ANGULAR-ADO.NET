using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Models.DTOs;

namespace EmployeeAdminPortal.Api.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<Employee>> GetEmployeesAsync(EmployeeSearchDto searchDto);

    Task<Employee?> GetByIdAsync(int id);

    Task AddAsync(AddEmployeeDto dto);

    Task UpdateAsync(int id, UpdateEmployeeDto dto);

    Task DeleteAsync(int id);
}