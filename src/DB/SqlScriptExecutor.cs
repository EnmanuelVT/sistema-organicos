using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace DB;

public class SqlScriptExecutor
{
    public void ExecuteSqlScript(string connectionString, string sqlFilePath)
    {
        var script = File.ReadAllText(sqlFilePath);

        // Split script on "GO" (case-insensitive, on a line by itself)
        string[] batches = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            foreach (var batch in batches)
            {
                string trimmedBatch = batch.Trim();
                if (!string.IsNullOrEmpty(trimmedBatch))
                {
                    using (SqlCommand cmd = new SqlCommand(trimmedBatch, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}