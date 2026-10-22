using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAdminPortal.Api.Data;

public class SqlConnectionFactory
{
    private readonly IConfiguration configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
}