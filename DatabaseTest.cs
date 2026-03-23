using Microsoft.Data.SqlClient;
using System;

class DatabaseTest
{
    static void Main()
    {
        string connectionString = "Server=.\\SQLEXPRESS;Database=QuantityMeasurementDB;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=5;";
        
        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            
            Console.WriteLine("✅ Database connection successful!");
            
            // Check if table exists
            using var command = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuantityMeasurements'", connection);
            var tableExists = command.ExecuteScalar() != null;
            
            if (tableExists)
            {
                Console.WriteLine("✅ Table 'QuantityMeasurements' exists!");
                
                // Try to create a test record
                using var insertCommand = new SqlCommand("INSERT INTO QuantityMeasurements (Operation, FirstValue, FirstUnit, SecondValue, SecondUnit, Result) VALUES ('TEST', 1, 'FEET', 2, 'FEET', 3)", connection);
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("✅ Test record inserted successfully!");
            }
            else
            {
                Console.WriteLine("❌ Table 'QuantityMeasurements' does not exist!");
                
                // Try to create the table
                using var createTableCommand = new SqlCommand(@"
                    CREATE TABLE QuantityMeasurements (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Operation NVARCHAR(50) NOT NULL,
                        FirstValue FLOAT NOT NULL,
                        FirstUnit NVARCHAR(50) NOT NULL,
                        SecondValue FLOAT NOT NULL,
                        SecondUnit NVARCHAR(50) NOT NULL,
                        Result NVARCHAR(50) NOT NULL,
                        MeasurementType NVARCHAR(50) NOT NULL
                    )", connection);
                createTableCommand.ExecuteNonQuery();
                Console.WriteLine("✅ Table 'QuantityMeasurements' created successfully!");
            }
            
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Database error: {ex.Message}");
        }
    }
}
