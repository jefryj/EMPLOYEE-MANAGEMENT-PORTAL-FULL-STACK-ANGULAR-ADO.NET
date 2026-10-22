using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;

namespace EmployeeAdminPortal.Api.Services.Interfaces;

public interface IDepartmentService
{
    Task<List<Department>> GetDepartmentsAsync();

    Task<Department?> GetByIdAsync(int id);

    Task AddAsync(AddDepartmentDto dto);

    Task UpdateAsync(int id, UpdateDepartmentDto dto);

    Task DeleteAsync(int id);
}