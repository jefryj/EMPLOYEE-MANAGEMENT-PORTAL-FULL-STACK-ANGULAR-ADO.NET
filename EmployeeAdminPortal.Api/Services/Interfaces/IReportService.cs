using EmployeeAdminPortal.Api.Models.DTOs;

namespace EmployeeAdminPortal.Api.Services.Interfaces;

public interface IReportService
{
	Task<EmployeeSummaryDto> GetEmployeeSummaryAsync();
	Task<List<DepartmentCountDto>> GetDepartmentCountAsync();
	Task<List<DepartmentSalarySummaryDto>> GetDepartmentSalarySummaryAsync();
	Task<List<ProjectCountDto>> GetProjectCountAsync();
	Task<List<ProjectSalarySummaryDto>> GetProjectSalarySummaryAsync();
}
