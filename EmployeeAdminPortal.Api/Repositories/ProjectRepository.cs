using System.Data;
using EmployeeAdminPortal.Api.Data;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace EmployeeAdminPortal.Api.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly SqlConnectionFactory factory;

    public ProjectRepository(SqlConnectionFactory factory)
    {
        this.factory = factory;
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        var projects = new List<Project>();

        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetProjects", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            projects.Add(new Project
            {
                Id = Convert.ToInt32(reader["Id"]),
                ProjectName = reader["ProjectName"].ToString() ?? string.Empty
            });
        }

        return projects;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetProjectById", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Project
            {
                Id = Convert.ToInt32(reader["Id"]),
                ProjectName = reader["ProjectName"].ToString() ?? string.Empty
            };
        }

        return null;
    }

    public async Task<bool> ExistsByNameAsync(string projectName)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("AnyProjectByName", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@ProjectName", projectName);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result) > 0;
    }

    public async Task AddAsync(Project project)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("AddProject", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@ProjectName", project.ProjectName);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(Project project)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("UpdateProject", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", project.Id);

        command.Parameters.AddWithValue("@ProjectName", project.ProjectName);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("DeleteProject", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
}