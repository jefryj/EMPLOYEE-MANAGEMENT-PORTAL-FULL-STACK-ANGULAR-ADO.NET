using EmployeeAdminPortal.Api.Models.Entities;

namespace EmployeeAdminPortal.Api.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetDepartmentsAsync();

    Task<Department?> GetByIdAsync(int id);

    Task<bool> ExistsByNameAsync(string departmentName);

    Task AddAsync(Department department);

    Task UpdateAsync(Department department);

    Task DeleteAsync(int id);
}