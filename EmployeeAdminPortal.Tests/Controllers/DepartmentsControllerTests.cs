using EmployeeAdminPortal.Api.Controllers;
using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeAdminPortal.Tests.Controllers;

public class DepartmentsControllerTests
{
    private readonly Mock<IDepartmentService> departmentServiceMock;
    private readonly DepartmentsController controller;

    public DepartmentsControllerTests()
    {
        departmentServiceMock = new Mock<IDepartmentService>();

        controller = new DepartmentsController(departmentServiceMock.Object);
    }
    [Fact]
    public async Task GetDepartments_ReturnsDepartments()
    {
        var departments = new List<Department>
        {
            new()
            {
                Id = 1,
                DepartmentName = "IT"
            }
        };

        departmentServiceMock.Setup(x => x.GetDepartmentsAsync()).ReturnsAsync(departments);

        var result = await controller.GetDepartments();

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedDepartments =Assert.IsAssignableFrom<List<Department>>(okResult.Value);

        Assert.Single(returnedDepartments);
    }
    [Fact]
    public async Task GetDepartmentById_ReturnsDepartment_WhenDepartmentExists()
    {
        var department = new Department
        {
            Id = 1,
            DepartmentName = "IT"
        };

        departmentServiceMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(department);

        var result = await controller.GetDepartmentById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedDepartment =Assert.IsType<Department>(okResult.Value);

        Assert.Equal(1, returnedDepartment.Id);
    }
    [Fact]
    public async Task AddDepartment_ReturnsOk()
    {
        var dto = new AddDepartmentDto
        {
            DepartmentName = "IT"
        };

        var result = await controller.AddDepartment(dto);

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task UpdateDepartment_ReturnsOkObjectResult()
    {
        var dto = new UpdateDepartmentDto
        {
            DepartmentName = "Development"
        };

        var result = await controller.UpdateDepartment(1, dto);

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task DeleteDepartment_ReturnsOkObjectResult()
    {
        var result = await controller.DeleteDepartment(1);

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task UpdateDepartment_CallsService()
    {
        var dto = new UpdateDepartmentDto
        {
            DepartmentName = "Development"
        };

        await controller.UpdateDepartment(1, dto);

        departmentServiceMock.Verify( x => x.UpdateAsync(1, dto),Times.Once);
    }
    [Fact]
    public async Task DeleteDepartment_CallsService()
    {
        await controller.DeleteDepartment(1);

        departmentServiceMock.Verify( x => x.DeleteAsync(1), Times.Once);
    }


}