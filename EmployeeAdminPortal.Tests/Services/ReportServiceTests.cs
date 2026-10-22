using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services;
using Moq;
using Microsoft.Extensions.Logging;

namespace EmployeeAdminPortal.Tests.Services;

public class ReportServiceTests
{
    private readonly Mock<IReportRepository> reportRepositoryMock;
    private readonly Mock<ILogger<ReportService>> loggerMock;
    private readonly ReportService reportService;

    public ReportServiceTests()
    {
        reportRepositoryMock = new Mock<IReportRepository>();

        loggerMock = new Mock<ILogger<ReportService>>();
        reportService = new ReportService(reportRepositoryMock.Object, loggerMock.Object);
    }
    [Fact]
    public async Task GetEmployeeSummaryAsync_ReturnsSummary()
    {
        var summary = new EmployeeSummaryDto();

        reportRepositoryMock.Setup(x => x.GetEmployeeSummaryAsync()).ReturnsAsync(summary);

        var result = await reportService.GetEmployeeSummaryAsync();

        Assert.NotNull(result);
    }
    [Fact]
    public async Task GetDepartmentCountAsync_ReturnsDepartments()
    {
        var departments = new List<DepartmentCountDto>
        {
            new()
        };

        reportRepositoryMock.Setup(x => x.GetDepartmentCountAsync()).ReturnsAsync(departments);

        var result = await reportService.GetDepartmentCountAsync();

        Assert.Single(result);
    }
    [Fact]
    public async Task GetDepartmentSalarySummaryAsync_ReturnsDepartments()
    {
        var departments = new List<DepartmentSalarySummaryDto>
        {
            new()
        };

        reportRepositoryMock.Setup(x => x.GetDepartmentSalarySummaryAsync()).ReturnsAsync(departments);

        var result = await reportService.GetDepartmentSalarySummaryAsync();

        Assert.Single(result);
    }
    [Fact]
    public async Task GetProjectCountAsync_ReturnsProjects()
    {
        var projects = new List<ProjectCountDto>
        {
            new()
        };

        reportRepositoryMock.Setup(x => x.GetProjectCountAsync()).ReturnsAsync(projects);

        var result = await reportService.GetProjectCountAsync();

        Assert.Single(result);
    }
    [Fact]
    public async Task GetProjectSalarySummaryAsync_ReturnsProjects()
    {
        var projects = new List<ProjectSalarySummaryDto>
        {
            new()
        };

        reportRepositoryMock.Setup(x => x.GetProjectSalarySummaryAsync()).ReturnsAsync(projects);

        var result =await reportService.GetProjectSalarySummaryAsync();

        Assert.Single(result);
    }


}