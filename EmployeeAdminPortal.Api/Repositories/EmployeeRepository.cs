using System.Data;
using EmployeeAdminPortal.Api.Data;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Models.DTOs;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace EmployeeAdminPortal.Api.Repositories;

public class EmployeeRepository:  IEmployeeRepository
{
    private readonly SqlConnectionFactory factory;

    public EmployeeRepository(SqlConnectionFactory factory)
    {
        this.factory = factory;
    }

    public async Task<List<Employee>> GetEmployeesAsync(EmployeeSearchDto searchDto)
    {
        var employees = new List<Employee>();

        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetEmployees",connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Name",(object?)searchDto.Name ?? DBNull.Value);

        command.Parameters.AddWithValue("@Email",(object?)searchDto.Email ?? DBNull.Value);

        command.Parameters.AddWithValue("@DepartmentId",(object?)searchDto.DepartmentId ?? DBNull.Value);

        command.Parameters.AddWithValue("@ProjectId",(object?)searchDto.ProjectId ?? DBNull.Value);

        command.Parameters.AddWithValue("@SortBy",(object?)searchDto.SortBy ?? DBNull.Value);

        command.Parameters.AddWithValue("@IsDescending",searchDto.IsDescending);

        command.Parameters.AddWithValue("@PageNumber",searchDto.PageNumber);

        command.Parameters.AddWithValue("@PageSize",searchDto.PageSize);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            employees.Add(new Employee
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Phone = reader["Phone"]?.ToString(),
                Salary = Convert.ToDecimal(reader["Salary"]),
                DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty,
                ProjectId = reader["ProjectId"] == DBNull.Value? null: Convert.ToInt32(reader["ProjectId"]),
                ProjectName = reader["ProjectName"] == DBNull.Value ? null : reader["ProjectName"].ToString(),
                RoleId = Convert.ToInt32(reader["RoleId"]),
                RoleName = reader["RoleName"].ToString() ?? string.Empty
            });
        }

        return employees;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetEmployeeById",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
        return new Employee
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Phone = reader["Phone"]?.ToString(),
                Salary = Convert.ToDecimal(reader["Salary"]),
                PasswordHash = reader["PasswordHash"].ToString() ?? string.Empty,

                DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty,

                ProjectId = reader["ProjectId"] == DBNull.Value? null: Convert.ToInt32(reader["ProjectId"]),

                ProjectName = reader["ProjectName"] == DBNull.Value ? null : reader["ProjectName"].ToString(),

                RoleId = Convert.ToInt32(reader["RoleId"]),
                RoleName = reader["RoleName"].ToString() ?? string.Empty
            };
        }

        return null;
    }

    public async Task<bool> AnyByEmailAsync(string email)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("AnyEmployeeByEmail",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Email", email);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result) > 0;
    }

    public async Task AddAsync(Employee employee)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("AddEmployee",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Name", employee.Name);
        command.Parameters.AddWithValue("@Email", employee.Email);
        command.Parameters.AddWithValue("@Phone", employee.Phone);
        command.Parameters.AddWithValue("@Salary", employee.Salary);
        command.Parameters.AddWithValue("@PasswordHash", employee.PasswordHash);
        command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
        command.Parameters.AddWithValue("@ProjectId",(object?)employee.ProjectId ?? DBNull.Value);
        command.Parameters.AddWithValue("@RoleId", employee.RoleId);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
    public async Task UpdateAsync(Employee employee)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("UpdateEmployee",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", employee.Id);
        command.Parameters.AddWithValue("@Name", employee.Name);
        command.Parameters.AddWithValue("@Email", employee.Email);
        command.Parameters.AddWithValue("@Phone", employee.Phone);
        command.Parameters.AddWithValue("@Salary", employee.Salary);
        command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
        command.Parameters.AddWithValue("@ProjectId",(object?)employee.ProjectId ?? DBNull.Value);
        command.Parameters.AddWithValue("@RoleId", employee.RoleId);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("DeleteEmployee",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
}
