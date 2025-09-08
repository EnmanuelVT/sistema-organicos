using DB;
using Microsoft.Extensions.Configuration;


// var connectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=YourStrong!Passw0rd;Trust Server Certificate=True";
// "Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=YourTrust Server Certificate=True" 
// var connectionString = "Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=YourStrong!Passw0rd;Trust Server Certificate=True;";
// var connectionString = "Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=YourStrong!Passw0rd;Encrypt=False;";
 var connectionString = "Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;";
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