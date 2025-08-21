// See https://aka.ms/new-console-template for more information

using DB;

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
// var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Missing CONNECTION_STRING environment variable.");
    return;
}

var executor = new SqlScriptExecutor();
executor.ExecuteSqlScript(
    connectionString,
    "/scripts/init-procedures.sql"
);