using EmployeeAdminPortal.Api.Models.Entities;

namespace EmployeeAdminPortal.Api.Repositories.Interfaces;

public interface IProjectRepository
{
	Task<List<Project>> GetProjectsAsync();
	Task<Project?> GetByIdAsync(int id);
	Task<bool> ExistsByNameAsync(string projectName);
	Task AddAsync(Project project);
	Task UpdateAsync(Project project);
	Task DeleteAsync(int id);
}
