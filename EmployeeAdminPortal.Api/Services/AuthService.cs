using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using EmployeeAdminPortal.Api.Models.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeAdminPortal.Api.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository authRepository;
    private readonly JwtSettings jwtSettings;
    private readonly IAuditLogRepository auditLogRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly ILogger<AuthService> logger;

    private readonly IEmployeeRepository employeeRepository;

    public AuthService(IAuthRepository authRepository, IOptions<JwtSettings> jwtOptions,
        IAuditLogRepository auditLogRepository, ICurrentUserService currentUserService,
        ILogger<AuthService> logger, IEmployeeRepository employeeRepository)
    {
        this.authRepository = authRepository;
        jwtSettings = jwtOptions.Value;
        this.auditLogRepository = auditLogRepository;
        this.currentUserService = currentUserService;
        this.logger = logger;
        this.employeeRepository = employeeRepository;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var employee = await authRepository.GetEmployeeByEmailAsync(dto.Email);
        if(employee == null)
        {   
            logger.LogWarning("Employee with email {Email} not found", dto.Email);
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        var passwordHasher = new PasswordHasher<Employee>();

        var verificationResult = passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, dto.Password);

        logger.LogInformation("Login verification result: {Result}",verificationResult);

        if(verificationResult == PasswordVerificationResult.Failed)
        {   
            logger.LogWarning("Employee {Email} provided an incorrect password", dto.Email);
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        logger.LogInformation("Employee {Email} logged in successfully", employee.Email);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,employee.Id.ToString()),

            new Claim(ClaimTypes.Email,employee.Email),

            new Claim(ClaimTypes.Role,employee.RoleId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
        logger.LogInformation("JWT token generated successfully for employee {Email}", employee.Email);
        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RoleId = employee.RoleId,
            EmployeeId = employee.Id
        };
    }
    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        var employee =await employeeRepository.GetByIdAsync(dto.EmployeeId);
    
        if (employee is null)
        {   
            logger.LogWarning("Employee with ID {EmployeeId} not found", dto.EmployeeId);
            throw new KeyNotFoundException("Employee not found.");
        }

        var passwordHasher = new PasswordHasher<Employee>();

        var verifyResult = passwordHasher.VerifyHashedPassword(employee,employee.PasswordHash,dto.CurrentPassword);

        logger.LogInformation("Change password verification result: {Result}",    verifyResult);

        if (verifyResult == PasswordVerificationResult.Failed)
        {
            logger.LogWarning("Employee {EmployeeId} provided an incorrect current password", dto.EmployeeId);
            throw new UnauthorizedAccessException("Current password is incorrect.");
        }

        var newPasswordHash = passwordHasher.HashPassword(employee,dto.NewPassword);

        await authRepository.ChangePasswordAsync(employee.Id, newPasswordHash);

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "ChangePassword",
            EntityName = "Employee",
            Details = "Password changed"
        });
        logger.LogInformation("Employee {EmployeeId} changed their password successfully", dto.EmployeeId);
    }
}