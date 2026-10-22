using EmployeeAdminPortal.Api.Controllers;
using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeAdminPortal.Tests.Controllers;

public class EmployeesControllerTests
{
    private readonly Mock<IEmployeeService> employeeServiceMock;
    private readonly EmployeesController controller;

    public EmployeesControllerTests()
    {
        employeeServiceMock = new Mock<IEmployeeService>();

        controller = new EmployeesController(employeeServiceMock.Object);
    }

    [Fact]
    public async Task GetEmployees_ReturnsEmployees()
    {

        var employees = new List<Employee>
        {
            new()
            {
                Id = 1,
                Name = "Jefry",
                Email = "jefry@test.com"
            }
        };

        employeeServiceMock.Setup(x => x.GetEmployeesAsync(It.IsAny<EmployeeSearchDto>())).ReturnsAsync(employees);

        var result = await controller.GetEmployees(new EmployeeSearchDto());

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedEmployees =Assert.IsAssignableFrom<List<Employee>>(okResult.Value);

        Assert.Single(returnedEmployees);
    }
    [Fact]
    public async Task GetById_ReturnsEmployee_WhenEmployeeExists()
    {

        var employee = new Employee
        {
            Id = 1,
            Name = "Jefry",
            Email = "jefry@test.com"
        };

        employeeServiceMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(employee);

        var result = await controller.GetEmployeeById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedEmployee =Assert.IsType<Employee>(okResult.Value);

        Assert.Equal(1, returnedEmployee.Id);
    }
    [Fact]
    public async Task AddEmployee_ReturnsOk()
    {
        var dto = new AddEmployeeDto
        {
            Name = "Jefry",
            Email = "jefry@test.com",
            Phone = "1234567890",
            Salary = 50000,
            DepartmentId = 1,
            Password = "Password@123",
            RoleId = 1
        };

        var result = await controller.AddEmployee(dto);

        Assert.IsType<OkResult>(result);
    }
    [Fact]
    public async Task UpdateEmployee_ReturnsOk()
    {
        var dto = new UpdateEmployeeDto
        {
            Name = "Updated",
            Email = "updated@test.com",
            Phone = "1234567890",
            Salary = 50000,
            DepartmentId = 1
        };

        var result = await controller.UpdateEmployee(1, dto);

        Assert.IsType<OkObjectResult>(result);
    }

    
    [Fact]
    public async Task UpdateEmployee_CallsService()
    {
        var dto = new UpdateEmployeeDto
        {
            Name = "Updated",
            Email = "updated@test.com",
            Phone = "1234567890",
            Salary = 50000,
            DepartmentId = 1
        };

        await controller.UpdateEmployee(1, dto);

        employeeServiceMock.Verify(x => x.UpdateAsync(1, dto),Times.Once);
    }

    [Fact]
    public async Task DeleteEmployee_ReturnsOk()
    {
        var result = await controller.DeleteEmployee(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteEmployee_CallsService()
    {
        await controller.DeleteEmployee(1);

        employeeServiceMock.Verify(x => x.DeleteAsync(1),Times.Once);
    }

}