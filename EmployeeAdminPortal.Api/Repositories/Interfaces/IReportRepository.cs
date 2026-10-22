using EmployeeAdminPortal.Api.Models.DTOs;

namespace EmployeeAdminPortal.Api.Repositories.Interfaces;

public interface IReportRepository
{
	Task<EmployeeSummaryDto> GetEmployeeSummaryAsync();
	Task<List<DepartmentCountDto>> GetDepartmentCountAsync();
	Task<List<DepartmentSalarySummaryDto>> GetDepartmentSalarySummaryAsync();
	Task<List<ProjectCountDto>> GetProjectCountAsync();
	Task<List<ProjectSalarySummaryDto>> GetProjectSalarySummaryAsync();
}
