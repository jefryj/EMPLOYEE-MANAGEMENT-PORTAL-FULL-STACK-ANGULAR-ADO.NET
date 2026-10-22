using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmployeeAdminPortal.Api.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository reportRepository;
    private readonly ILogger<ReportService> logger;

    public ReportService(IReportRepository reportRepository, ILogger<ReportService> logger)
    {
        this.reportRepository = reportRepository;
        this.logger = logger;
    }

    public async Task<EmployeeSummaryDto> GetEmployeeSummaryAsync()
    {   
        logger.LogInformation("Fetching employee summary.");
        return await reportRepository.GetEmployeeSummaryAsync();
    }

    public async Task<List<DepartmentCountDto>> GetDepartmentCountAsync()
    {
        logger.LogInformation("Fetching department count.");
        return await reportRepository.GetDepartmentCountAsync();
    }

    public async Task<List<DepartmentSalarySummaryDto>> GetDepartmentSalarySummaryAsync()
    {
        logger.LogInformation("Fetching department salary summary.");
        return await reportRepository.GetDepartmentSalarySummaryAsync();
    }

    public async Task<List<ProjectCountDto>> GetProjectCountAsync()
    {
        logger.LogInformation("Fetching project count.");
        return await reportRepository.GetProjectCountAsync();
    }

    public async Task<List<ProjectSalarySummaryDto>> GetProjectSalarySummaryAsync()
    {
        logger.LogInformation("Fetching project salary summary.");
        return await reportRepository.GetProjectSalarySummaryAsync();
    }
}