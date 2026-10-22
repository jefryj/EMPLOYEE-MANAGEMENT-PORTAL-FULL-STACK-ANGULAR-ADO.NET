using EmployeeAdminPortal.Api.Controllers;
using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;

namespace EmployeeAdminPortal.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> authServiceMock;
    private readonly AuthController controller;

    public AuthControllerTests()
    {
        authServiceMock = new Mock<IAuthService>();

        controller = new AuthController(authServiceMock.Object);
    }

    [Fact]
    public async Task Login_ReturnsOkObjectResult()
    {
        var response = new LoginResponseDto
        {
            Token = "test-token"
        };

        authServiceMock.Setup(x => x.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(response);

        var result = await controller.Login(
            new LoginDto
            {
                Email = "jefry@test.com",
                Password = "Password123"
            });

        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedResponse =Assert.IsType<LoginResponseDto>(okResult.Value);

        Assert.Equal("test-token", returnedResponse.Token);
    }

    [Fact]
    public async Task Login_CallsService()
    {
        var dto = new LoginDto
        {
            Email = "jefry@test.com",
            Password = "Password123"
        };

        authServiceMock.Setup(x => x.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(new LoginResponseDto());

        await controller.Login(dto);

        authServiceMock.Verify( x => x.LoginAsync(dto), Times.Once);
    }

    [Fact]
    public async Task ChangePassword_ReturnsOkObjectResult()
    {
        var dto = new ChangePasswordDto
        {
            EmployeeId = 1,
            CurrentPassword = "OldPassword",
            NewPassword = "NewPassword"
        };

        var result =await controller.ChangePassword(dto);

        var okResult =Assert.IsType<OkObjectResult>(result);

        var response = JsonSerializer.SerializeToElement(okResult.Value);

        Assert.Equal("Password changed successfully.",response.GetProperty("message").GetString());
    }

    [Fact]
    public async Task ChangePassword_CallsService()
    {
        var dto = new ChangePasswordDto
        {
            EmployeeId = 1,
            CurrentPassword = "OldPassword",
            NewPassword = "NewPassword"
        };

        await controller.ChangePassword(dto);

        authServiceMock.Verify(x => x.ChangePasswordAsync(dto),Times.Once);
    }
}