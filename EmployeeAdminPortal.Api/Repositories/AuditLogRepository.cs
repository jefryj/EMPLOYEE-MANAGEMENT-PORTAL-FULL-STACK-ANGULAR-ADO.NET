using System.Data;
using EmployeeAdminPortal.Api.Data;
using EmployeeAdminPortal.Api.Models.Entities;
using EmployeeAdminPortal.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace EmployeeAdminPortal.Api.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly SqlConnectionFactory factory;

    public AuditLogRepository(SqlConnectionFactory factory)
    {
        this.factory = factory;
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("AddAuditLog", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@EmployeeId", auditLog.EmployeeId);
        command.Parameters.AddWithValue("@UserName", auditLog.UserName);
        command.Parameters.AddWithValue("@Action", auditLog.Action);
        command.Parameters.AddWithValue("@EntityName", auditLog.EntityName);
        command.Parameters.AddWithValue("@Details", auditLog.Details);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
    public async Task<List<AuditLog>> GetAuditLogsAsync()
    {
        var auditLogs = new List<AuditLog>();

        using var connection = factory.CreateConnection();

        using var command = new SqlCommand("GetAuditLogs", connection);

        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            auditLogs.Add(new AuditLog
            {
                Id = Convert.ToInt32(reader["Id"]),
                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                UserName = reader["UserName"]?.ToString() ?? string.Empty,
                Action = reader["Action"]?.ToString() ?? string.Empty,
                EntityName = reader["EntityName"]?.ToString() ?? string.Empty,
                Details = reader["Details"]?.ToString() ?? string.Empty,
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            });
        }

        return auditLogs;
    }
}