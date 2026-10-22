using EmployeeAdminPortal.Api.Controllers;
using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeAdminPortal.Tests.Controllers;

public class ReportsControllerTests
{
    private readonly Mock<IReportService> reportServiceMock;
    private readonly ReportsController controller;

    public ReportsControllerTests()
    {
        reportServiceMock = new Mock<IReportService>();

        controller = new ReportsController(
            reportServiceMock.Object);
    }
    [Fact]
    public async Task GetEmployeeSummary_ReturnsOkObjectResult()
    {
        reportServiceMock.Setup(x => x.GetEmployeeSummaryAsync()).ReturnsAsync(new EmployeeSummaryDto());

        var result = await controller.GetEmployeeSummary();

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task GetDepartmentCount_ReturnsOkObjectResult()
    {
        reportServiceMock.Setup(x => x.GetDepartmentCountAsync()).ReturnsAsync(new List<DepartmentCountDto>());

        var result = await controller.GetDepartmentCount();

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task GetDepartmentSalarySummary_ReturnsOkObjectResult()
    {
        reportServiceMock.Setup(x => x.GetDepartmentSalarySummaryAsync()).ReturnsAsync(new List<DepartmentSalarySummaryDto>());

        var result = await controller.GetDepartmentSalarySummary();

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task GetProjectCount_ReturnsOkObjectResult()
    {
        reportServiceMock.Setup(x => x.GetProjectCountAsync()).ReturnsAsync(new List<ProjectCountDto>());

        var result = await controller.GetProjectCount();

        Assert.IsType<OkObjectResult>(result);
    }
        [Fact]
    public async Task GetProjectSalarySummary_ReturnsOkObjectResult()
    {
        reportServiceMock.Setup(x => x.GetProjectSalarySummaryAsync()).ReturnsAsync(new List<ProjectSalarySummaryDto>());

        var result =await controller.GetProjectSalarySummary();

        Assert.IsType<OkObjectResult>(result);
    }
}