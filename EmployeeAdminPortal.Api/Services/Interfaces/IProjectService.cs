using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;

namespace EmployeeAdminPortal.Api.Services.Interfaces;

public interface IProjectService
{
	Task<List<Project>> GetProjectsAsync();
	Task<Project?> GetByIdAsync(int id);
	Task AddAsync(AddProjectDto dto);
	Task UpdateAsync(int id, UpdateProjectDto dto);
	Task DeleteAsync(int id);
}
