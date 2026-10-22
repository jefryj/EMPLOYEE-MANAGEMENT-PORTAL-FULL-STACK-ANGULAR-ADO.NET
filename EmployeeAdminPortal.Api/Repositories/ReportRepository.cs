using System.Data;
using EmployeeAdminPortal.Api.Data;
using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace EmployeeAdminPortal.Api.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly SqlConnectionFactory factory;

    public ReportRepository(SqlConnectionFactory factory)
    {
        this.factory = factory;
    }

    public async Task<EmployeeSummaryDto> GetEmployeeSummaryAsync()
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetEmployeeSummary", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new EmployeeSummaryDto
            {
                TotalEmployees = Convert.ToInt32(reader["TotalEmployees"]),
                AverageSalary = Convert.ToDecimal(reader["AverageSalary"]),
                MinSalary = Convert.ToDecimal(reader["MinSalary"]),
                MaxSalary = Convert.ToDecimal(reader["MaxSalary"])
            };
        }

        return new EmployeeSummaryDto();
    }

    public async Task<List<DepartmentCountDto>> GetDepartmentCountAsync()
    {
        var departments = new List<DepartmentCountDto>();

        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetDepartmentCount", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(new DepartmentCountDto
            {
                DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty,
                EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
            });
        }

        return departments;
    }

    public async Task<List<DepartmentSalarySummaryDto>> GetDepartmentSalarySummaryAsync()
    {
        var departments = new List<DepartmentSalarySummaryDto>();

        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetDepartmentSalarySummary", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(new DepartmentSalarySummaryDto
            {
                DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty,
                EmployeeCount = Convert.ToInt32(reader["EmployeeCount"]),
                AverageSalary = Convert.ToDecimal(reader["AverageSalary"]),
                MinSalary = Convert.ToDecimal(reader["MinSalary"]),
                MaxSalary = Convert.ToDecimal(reader["MaxSalary"])
            });
        }

        return departments;
    }

    public async Task<List<ProjectCountDto>> GetProjectCountAsync()
    {
        var projects = new List<ProjectCountDto>();

        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetProjectCount", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            projects.Add(new ProjectCountDto
            {
                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                ProjectName = reader["ProjectName"].ToString() ?? string.Empty,
                EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
            });
        }

        return projects;
    }

    public async Task<List<ProjectSalarySummaryDto>> GetProjectSalarySummaryAsync()
    {
        var projects = new List<ProjectSalarySummaryDto>();

        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetProjectSalarySummary", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            projects.Add(new ProjectSalarySummaryDto
            {
                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                ProjectName = reader["ProjectName"].ToString() ?? string.Empty,
                EmployeeCount = Convert.ToInt32(reader["EmployeeCount"]),
                AverageSalary = Convert.ToDecimal(reader["AverageSalary"]),
                MinSalary = Convert.ToDecimal(reader["MinSalary"]),
                MaxSalary = Convert.ToDecimal(reader["MaxSalary"])
            });
        }

        return projects;
    }
}