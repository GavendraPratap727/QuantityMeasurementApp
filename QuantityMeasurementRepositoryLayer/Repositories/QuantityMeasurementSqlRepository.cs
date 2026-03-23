using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuantityMeasurementModelLayer.Entities;
using QuantityMeasurementRepositoryLayer.Interfaces;

namespace QuantityMeasurementRepositoryLayer.Repositories;

public class QuantityMeasurementSqlRepository : IQuantityMeasurementRepositorySql
{
    private readonly string _connectionString;

    public QuantityMeasurementSqlRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public void Save(QuantityMeasurementEntity entity)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("INSERT INTO [QuantityMeasurements] ([FirstUnit], [FirstValue], [Operation], [Result], [SecondUnit], [SecondValue], [MeasurementType]) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)", connection);
            command.Parameters.AddWithValue("@p0", entity.FirstUnit);
            command.Parameters.AddWithValue("@p1", entity.FirstValue);
            command.Parameters.AddWithValue("@p2", entity.Operation);
            command.Parameters.AddWithValue("@p3", entity.Result);
            command.Parameters.AddWithValue("@p4", entity.SecondUnit);
            command.Parameters.AddWithValue("@p5", entity.SecondValue);
            command.Parameters.AddWithValue("@p6", entity.MeasurementType);
            command.ExecuteNonQuery();
        }
    }

    public List<QuantityMeasurementEntity> GetAll()
    {
        var entities = new List<QuantityMeasurementEntity>();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT Operation, Operand1, Operand2, Result FROM QuantityMeasurements", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    entities.Add(new QuantityMeasurementEntity(
                        reader.GetString(0),
                        reader.GetDouble(1),
                        reader.GetDouble(2),
                        reader.GetString(3)
                    ));
                }
            }
        }
        return entities;
    }
}