using System.Data;
using EmployeeAdminPortal.Api.Data;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
namespace EmployeeAdminPortal.Api.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly SqlConnectionFactory factory;

    public DepartmentRepository(SqlConnectionFactory factory)
    {
        this.factory = factory;
    }

   public async Task<List<Department>> GetDepartmentsAsync()
    {
        var departments = new List<Department>();

        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetDepartments",connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(new Department
            {
                Id = Convert.ToInt32(reader["Id"]),
                DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty
            });
        }

        return departments;
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetDepartmentById",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id",id);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Department
            {
                Id = Convert.ToInt32(reader["Id"]),
                DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty
            };
        }

        return null;
    }

    public async Task<bool> ExistsByNameAsync(string departmentName)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("AnyDepartmentByName",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@DepartmentName",departmentName);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result) > 0;
    }

    public async Task AddAsync(Department department)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("AddDepartment",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@DepartmentName",department.DepartmentName);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("UpdateDepartment",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", department.Id);

        command.Parameters.AddWithValue("@DepartmentName",department.DepartmentName);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("DeleteDepartment", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id",id);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
}
