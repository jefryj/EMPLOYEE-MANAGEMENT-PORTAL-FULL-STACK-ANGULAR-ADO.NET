using EmployeeAdminPortal.Api.Models.DTOs;

namespace EmployeeAdminPortal.Api.Services.Interfaces;

public interface IAuthService
{
    Task <LoginResponseDto> LoginAsync(LoginDto dto);

    Task ChangePasswordAsync(ChangePasswordDto dto);
}