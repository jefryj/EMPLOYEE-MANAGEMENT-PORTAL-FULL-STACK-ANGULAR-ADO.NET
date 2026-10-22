using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeAdminPortal.Api.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository departmentRepository;
    private readonly IAuditLogRepository auditLogRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IMemoryCache cache;
    private static readonly List<string> departmentCacheKeys = [];

    private readonly ILogger<DepartmentService> logger;

    public DepartmentService(IDepartmentRepository departmentRepository, IAuditLogRepository auditLogRepository, 
    ICurrentUserService currentUserService,
    IMemoryCache cache,
    ILogger<DepartmentService> logger)
    {
        this.departmentRepository = departmentRepository;
        this.auditLogRepository = auditLogRepository;
        this.currentUserService = currentUserService;
        this.cache = cache;
        this.logger = logger;
    }

    public async Task<List<Department>> GetDepartmentsAsync()
    {
        const string cacheKey = "departments";

        if (cache.TryGetValue(cacheKey, out List<Department>? departments))
        {
            logger.LogInformation("Returning cached departments for cache key: {CacheKey}", cacheKey);
            return departments!;
        }

        departments = await departmentRepository.GetDepartmentsAsync();

        cache.Set(cacheKey,departments,TimeSpan.FromMinutes(5));
        logger.LogInformation("Departments fetched successfully from the database");
        logger.LogInformation("Departments cached successfully for cache key: {CacheKey}", cacheKey);

        if (!departmentCacheKeys.Contains(cacheKey))
        {
            departmentCacheKeys.Add(cacheKey);
        }

        return departments;
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        var cacheKey = $"department-{id}";

        if (cache.TryGetValue(cacheKey, out Department? department))
        {
            logger.LogInformation("Returning cached department for cache key: {CacheKey}", cacheKey);
            return department;
        }

        department = await departmentRepository.GetByIdAsync(id);

        if (department != null)
        {
            cache.Set(cacheKey, department, TimeSpan.FromMinutes(5));
            logger.LogInformation("Department {DepartmentName} fetched successfully from the database", department?.DepartmentName);
            logger.LogInformation("Department cached successfully for cache key: {CacheKey}", cacheKey);
        }

        return department;
    }

    public async Task AddAsync(AddDepartmentDto dto)
    {
        bool exists = await departmentRepository.ExistsByNameAsync(dto.DepartmentName);

        if (exists)
        {   
            logger.LogWarning("Department with name {DepartmentName} already exists", dto.DepartmentName);
            throw new InvalidOperationException("Department already exists.");
        }

        var department = new Department
        {
            DepartmentName = dto.DepartmentName
        };

        await departmentRepository.AddAsync(department);

        RemoveDepartmentCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Create",
            EntityName = "Department",
            Details = $"Created department '{department.DepartmentName}'"
        });
        logger.LogInformation("Department {DepartmentName} created successfully", department.DepartmentName);
    }
    public async Task UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department= await departmentRepository.GetByIdAsync(id);
        if(department==null)
        {   
            logger.LogWarning("Department with id {DepartmentId} not found", id);
            throw new KeyNotFoundException("Department not found.");
        }

        var oldDepartmentName = department.DepartmentName;
        department.DepartmentName = dto.DepartmentName;

        await departmentRepository.UpdateAsync(department);

        cache.Remove($"department-{id}");
        RemoveDepartmentCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Update",
            EntityName = "Department",
            Details = $"Updated department from '{oldDepartmentName}' to '{department.DepartmentName}'"
        });
        logger.LogInformation("Department {DepartmentName} updated successfully", department.DepartmentName);
    }

    public async Task DeleteAsync(int id)
    {
        var department = await departmentRepository.GetByIdAsync(id);
        if (department == null)
        {   
            logger.LogWarning("Department with id {DepartmentId} not found", id);
            throw new KeyNotFoundException("Department not found.");
        }

        await departmentRepository.DeleteAsync(id);

        cache.Remove($"department-{id}");
        RemoveDepartmentCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Delete",
            EntityName = "Department",
            Details = $"Deleted department '{department.DepartmentName}'"
        });
        logger.LogInformation("Department {DepartmentName} deleted successfully", department.DepartmentName);
    }
    private void RemoveDepartmentCaches()
    {
        foreach (var key in departmentCacheKeys)
        {
            cache.Remove(key);
        }

        departmentCacheKeys.Clear();
    }
}
