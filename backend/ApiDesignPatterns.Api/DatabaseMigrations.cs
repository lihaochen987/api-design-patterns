// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using DbUp;
using DbUp.Engine;

namespace backend;

public static class DatabaseMigrations
{
    public static void Apply(
        string? connectionString,
        int retryCount,
        int delayInSeconds,
        string migrationPath)
    {
        if (!Directory.Exists(migrationPath))
        {
            string errorMessage = $"Migration path does not exist: {migrationPath}";
            Console.WriteLine(errorMessage);
            throw new DirectoryNotFoundException(errorMessage);
        }

        for (int attempt = 1; attempt <= retryCount; attempt++)
        {
            try
            {
                EnsureDatabase.For.PostgresqlDatabase(connectionString);
                UpgradeEngine? upgrader = DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsFromFileSystem(migrationPath)
                    .LogToConsole()
                    .Build();

                DatabaseUpgradeResult? result = upgrader.PerformUpgrade();
                if (!result.Successful)
                {
                    throw new Exception("Migration failed: " + result.Error);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("All migrations applied successfully.");
                Console.ResetColor();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
                if (attempt == retryCount)
                {
                    throw;
                }

                Console.WriteLine("Retrying in 2 seconds...");
                Thread.Sleep(delayInSeconds);
            }
        }
    }
}
