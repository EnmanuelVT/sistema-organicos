// See https://aka.ms/new-console-template for more information

using DB;
using DotNetEnv;

Env.Load(); // Loads .env file from the current directory

// Now environment variables will be available

// var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default");
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var trustServerCertificate = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE") ?? "True";

var connectionString = $"Server={dbServer};Database={dbDatabase};User Id={dbUser};Password={dbPassword};TrustServerCertificate={trustServerCertificate};";
Console.WriteLine($"Connection string: {connectionString}");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Missing CONNECTION_STRING environment variable.");
    return;
}

var executor = new SqlScriptExecutor();
try
{
    executor.ExecuteSqlScriptsInFolder(
        connectionString,
        "./scripts/"
    );
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while executing SQL scripts. Details:", ex.Message);
    return;
}