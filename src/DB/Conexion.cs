using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace DB;

public class Conexion
{
    private static readonly Lazy<Conexion> _instance = new Lazy<Conexion>(() => new Conexion());
    private readonly SqlConnection _connection;

    private Conexion()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = Environment.GetEnvironmentVariable("DB_SERVER"),
            InitialCatalog = Environment.GetEnvironmentVariable("DB_DATABASE"),
            UserID = Environment.GetEnvironmentVariable("DB_USER"),
            Password = Environment.GetEnvironmentVariable("DB_PASSWORD"),
            TrustServerCertificate = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE") == "True"
        };
        _connection = new SqlConnection(builder.ConnectionString);
    }

    public static Conexion Instance => _instance.Value;

    public SqlConnection Connection => _connection;
}