using System.Data;
using EmployeeAdminPortal.Api.Data;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace EmployeeAdminPortal.Api.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly SqlConnectionFactory factory;

    public AuthRepository(SqlConnectionFactory factory)
    {
        this.factory = factory;
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email)
    {
        using var connection =factory.CreateConnection();

        using var command = new SqlCommand( "GetEmployeeByEmail",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue( "@Email", email);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Employee
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString()?? string.Empty,
                Email = reader["Email"].ToString()?? string.Empty,
                PasswordHash = reader["PasswordHash"].ToString()?? string.Empty,
                RoleId = Convert.ToInt32(reader["RoleId"])
            };
        }

        return null;
    }

    public async Task ChangePasswordAsync(int employeeId, string passwordHash)
    {
        using var connection =factory.CreateConnection();

        using var command = new SqlCommand( "ChangePassword",connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@EmployeeId",employeeId);

        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
}
