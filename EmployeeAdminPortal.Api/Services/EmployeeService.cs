using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services.Interfaces;
using EmployeeAdminPortal.Api.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeAdminPortal.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository employeeRepository;
    private readonly IAuditLogRepository auditLogRepository;
    private readonly ICurrentUserService currentUserService;

    private readonly IMemoryCache cache;
    private static readonly List<string> employeeCacheKeys = [];

    private readonly ILogger<EmployeeService> logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IAuditLogRepository auditLogRepository,
        ICurrentUserService currentUserService,
        IMemoryCache cache,
        ILogger<EmployeeService> logger)
    {
        this.employeeRepository = employeeRepository;
        this.auditLogRepository = auditLogRepository;
        this.currentUserService = currentUserService;
        this.cache = cache;
        this.logger = logger;
    }

    public async Task<List<Employee>> GetEmployeesAsync(EmployeeSearchDto searchDto)
    {   
        logger.LogInformation("Fetching employees with search criteria: {@SearchDto}", searchDto);
        var cacheKey = $"employees-{searchDto.Name}-{searchDto.Email}-{searchDto.DepartmentId}-{searchDto.ProjectId}-{searchDto.SortBy}-{searchDto.IsDescending}-{searchDto.PageNumber}-{searchDto.PageSize}";

        if (!employeeCacheKeys.Contains(cacheKey))
        {   

            employeeCacheKeys.Add(cacheKey);
        }

        if (cache.TryGetValue(cacheKey, out List<Employee>? employees))
        {   
            logger.LogInformation("Returning cached employees for cache key: {CacheKey}", cacheKey);
            return employees!;
        }

        employees = await employeeRepository.GetEmployeesAsync(searchDto);

        cache.Set(cacheKey,employees,TimeSpan.FromMinutes(5));
        logger.LogInformation("Employees fetched successfully from the database");
        logger.LogInformation("Employees cached successfully for cache key: {CacheKey}", cacheKey);

        return employees;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        var cacheKey = $"employee-{id}";

        if (cache.TryGetValue(cacheKey, out Employee? employee))
        {   
            logger.LogInformation("Returning cached employee for cache key: {CacheKey}", cacheKey);
            return employee;
        }

        employee = await employeeRepository.GetByIdAsync(id);

        if (employee != null)
        {
            cache.Set(cacheKey, employee, TimeSpan.FromMinutes(5));
        }
        logger.LogInformation("Employee {EmployeeName} fetched successfully from the database", employee?.Name);

        return employee;
    }

    public async Task AddAsync(AddEmployeeDto dto)
    {
        bool emailexists = await employeeRepository.AnyByEmailAsync(dto.Email);
        if (emailexists)
        {
            throw new InvalidOperationException("An employee with the same email already exists.");
        }

        var employee = new Employee
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Salary = dto.Salary,
            DepartmentId = dto.DepartmentId,
            ProjectId = dto.ProjectId,
            RoleId = dto.RoleId
        };
        var passwordHasher=new PasswordHasher<Employee>();
        employee.PasswordHash = passwordHasher.HashPassword(employee, dto.Password);
        await employeeRepository.AddAsync(employee);
        logger.LogInformation("Employee {EmployeeName} created successfully",employee.Name);

        RemoveEmployeeListCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Create",
            EntityName = "Employee",
            Details = $"Created employee '{employee.Name}'"
        });
    }
    public async Task UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        if(employee is null)
        {   
            logger.LogWarning("Employee with id {EmployeeId} not found", id);
            throw new KeyNotFoundException("Employee not found.");
        }

        employee.Name = dto.Name;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.Salary = dto.Salary;
        employee.DepartmentId = dto.DepartmentId;
        employee.ProjectId = dto.ProjectId;

        await employeeRepository.UpdateAsync(employee);
        logger.LogInformation("Employee {EmployeeName} updated successfully", employee.Name);
        cache.Remove($"employee-{id}");
        RemoveEmployeeListCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Update",
            EntityName = "Employee",
            Details = $"Updated employee with id '{employee.Id}'"
        });
    }
    public async Task DeleteAsync(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        if(employee is null)
        {
            logger.LogWarning("Employee with id {EmployeeId} not found", id);
            throw new KeyNotFoundException("Employee not found.");
        }

        await employeeRepository.DeleteAsync(id);

        cache.Remove($"employee-{id}");
        RemoveEmployeeListCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Delete",
            EntityName = "Employee",
            Details = $"Deleted employee '{employee.Name}'"
        });
        logger.LogInformation("Employee {EmployeeName} deleted successfully", employee.Name);
    }

    private void RemoveEmployeeListCaches()
    {
        foreach (var key in employeeCacheKeys)
        {
            cache.Remove(key);
        }

        employeeCacheKeys.Clear();
    }

}