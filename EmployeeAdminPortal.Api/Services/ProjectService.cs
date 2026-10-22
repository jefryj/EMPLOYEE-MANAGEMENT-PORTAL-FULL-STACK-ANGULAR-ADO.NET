using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using EmployeeAdminPortal.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeAdminPortal.Api.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository projectRepository;
    private readonly IAuditLogRepository auditLogRepository;
    private readonly ICurrentUserService currentUserService;

    private readonly IMemoryCache cache;

    private static readonly List<string> projectCacheKeys = new();
    private readonly ILogger<ProjectService> logger;

    public ProjectService(IProjectRepository projectRepository, IAuditLogRepository auditLogRepository,
        ICurrentUserService currentUserService,
        IMemoryCache cache,
        ILogger<ProjectService> logger)
    {
        this.projectRepository = projectRepository;
        this.auditLogRepository = auditLogRepository;
        this.currentUserService = currentUserService;
        this.cache = cache;
        this.logger = logger;
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        var cacheKey = "projects";

        if (cache.TryGetValue(cacheKey, out List<Project>? projects))
        {
            logger.LogInformation("Returning cached projects for cache key: {CacheKey}", cacheKey);
            return projects!;
        }

        projects = await projectRepository.GetProjectsAsync();

        cache.Set(cacheKey, projects, TimeSpan.FromMinutes(5));

        if (!projectCacheKeys.Contains(cacheKey))
        {
            projectCacheKeys.Add(cacheKey);
        }

        return projects;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        var cacheKey = $"project-{id}";

        if (cache.TryGetValue(cacheKey, out Project? project))
        {
            logger.LogInformation("Returning cached project for cache key: {CacheKey}", cacheKey);
            return project;
        }

        project = await projectRepository.GetByIdAsync(id);

        if (project != null)
        {
            cache.Set(cacheKey, project, TimeSpan.FromMinutes(5));
            logger.LogInformation("Project {ProjectName} fetched successfully from the database", project?.ProjectName);
            logger.LogInformation("Project cached successfully for cache key: {CacheKey}", cacheKey);
        }

        return project;
    }

    public async Task AddAsync(AddProjectDto dto)
    {
        bool exists = await projectRepository.ExistsByNameAsync(dto.ProjectName);

        if (exists)
        {
            logger.LogWarning("Project with name {ProjectName} already exists", dto.ProjectName);
            throw new InvalidOperationException("Project already exists.");
        }

        var project = new Project
        {
            ProjectName = dto.ProjectName
        };

        await projectRepository.AddAsync(project);
        logger.LogInformation("Project {ProjectName} created successfully", project.ProjectName);
        logger.LogInformation("Project cached successfully for cache key: {CacheKey}", "projects");

        RemoveProjectCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Create",
            EntityName = "Project",
            Details = $"Created project '{project.ProjectName}'"
        });
    }

    public async Task UpdateAsync(int id, UpdateProjectDto dto)
    {
        var project = await projectRepository.GetByIdAsync(id);

        if (project == null)
        {
            throw new KeyNotFoundException("Project not found.");
        }

        var oldProjectName = project.ProjectName;
        project.ProjectName = dto.ProjectName;

        await projectRepository.UpdateAsync(project);
        logger.LogInformation("Project {ProjectName} updated successfully", project.ProjectName);

        cache.Remove($"project-{id}");
        RemoveProjectCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Update",
            EntityName = "Project",
            Details = $"Updated project from '{oldProjectName}' to '{project.ProjectName}'"
        });
    }

    public async Task DeleteAsync(int id)
    {
        var project = await projectRepository.GetByIdAsync(id);

        if (project == null)
        {   
            logger.LogWarning("Project with id {ProjectId} not found", id);
            throw new KeyNotFoundException("Project not found.");
        }

        await projectRepository.DeleteAsync(id);

        cache.Remove($"project-{id}");
        RemoveProjectCaches();

        await auditLogRepository.AddAsync(new AuditLog
        {
            EmployeeId = currentUserService.UserId,
            UserName = currentUserService.UserName,
            Action = "Delete",
            EntityName = "Project",
            Details = $"Deleted project '{project.ProjectName}'"
        });
        logger.LogInformation("Project {ProjectName} deleted successfully", project.ProjectName);
    }

    private void RemoveProjectCaches()
    {
        foreach (var key in projectCacheKeys)
        {
            cache.Remove(key);
        }

        projectCacheKeys.Clear();
    }
}