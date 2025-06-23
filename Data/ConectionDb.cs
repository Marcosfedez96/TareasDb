using Microsoft.Data.SqlClient;

namespace TareasDb.Data;

public class ConnectionDb
{
    private readonly IConfiguration _configuration;

    private readonly string? _connectionString;

    public ConnectionDb(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
