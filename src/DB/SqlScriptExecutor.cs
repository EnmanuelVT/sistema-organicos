using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace DB;

public class SqlScriptExecutor
{
    public void ExecuteSqlScriptsInFolder(string connectionString, string scriptsFolderPath)
    {
        var sqlFiles = Directory.GetFiles(scriptsFolderPath, "*.sql")
            .OrderBy(f => f)
            .ToList();

        if (sqlFiles.Count == 0)
        {
            Console.WriteLine($"No .sql files found in {scriptsFolderPath}");
            return;
        }

        Console.WriteLine($"Found {sqlFiles.Count} SQL file(s) to execute:");
        foreach (var file in sqlFiles)
        {
            Console.WriteLine($"  - {Path.GetFileName(file)}");
        }

        using (var conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                Console.WriteLine("Database connection established successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to database: {ex.Message}");
                throw;
            }

            foreach (var sqlFile in sqlFiles)
            {
                Console.WriteLine($"\nExecuting script: {Path.GetFileName(sqlFile)}");
                string script = File.ReadAllText(sqlFile);

                // Split on "GO" (case-insensitive, line by itself)
                string[] batches = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                Console.WriteLine($"Script contains {batches.Length} batch(es)");

                int batchNum = 0;
                int successfulBatches = 0;
                
                foreach (var batch in batches)
                {
                    string trimmedBatch = batch.Trim();
                    
                    // Skip empty batches
                    if (string.IsNullOrEmpty(trimmedBatch))
                        continue;
                    
                    // Check if batch contains only comments (no actual SQL commands)
                    string[] lines = trimmedBatch.Split('\n');
                    bool hasNonCommentContent = false;
                    
                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim();
                        // Skip empty lines and comment lines
                        if (string.IsNullOrEmpty(trimmedLine) || 
                            trimmedLine.StartsWith("--") || 
                            trimmedLine.StartsWith("/*") ||
                            trimmedLine.StartsWith("*") ||
                            trimmedLine.EndsWith("*/"))
                            continue;
                            
                        // If we find a non-comment line, this batch has actual SQL
                        hasNonCommentContent = true;
                        break;
                    }
                    
                    if (!hasNonCommentContent)
                    {
                        Console.WriteLine($"  Skipping batch {batchNum + 1}: Contains only comments");
                        continue;
                    }

                    batchNum++;
                    Console.WriteLine($"  Executing batch {batchNum}...");
                    
                    try
                    {
                        using (var cmd = new SqlCommand(trimmedBatch, conn))
                        {
                            cmd.CommandTimeout = 300; // 5 minutes timeout
                            int rowsAffected = cmd.ExecuteNonQuery();
                            Console.WriteLine($"  Batch {batchNum}: SUCCESS ({rowsAffected} rows affected)");
                            successfulBatches++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  Batch {batchNum}: ERROR - {ex.Message}");
                        Console.WriteLine($"  Failed SQL preview: {trimmedBatch.Substring(0, Math.Min(200, trimmedBatch.Length))}...");
                        // Don't throw here to continue with other batches, but log the error
                    }
                }
                
                Console.WriteLine($"Script execution completed: {successfulBatches}/{batchNum} batches successful");
            }
            
            // Verify tables were created
            Console.WriteLine("\nVerifying database objects...");
            try
            {
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", conn))
                {
                    var tableCount = cmd.ExecuteScalar();
                    Console.WriteLine($"Total tables in database: {tableCount}");
                }
                
                using (var cmd = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Tables created:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"  - {reader["TABLE_NAME"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying database objects: {ex.Message}");
            }
        }
    }
}