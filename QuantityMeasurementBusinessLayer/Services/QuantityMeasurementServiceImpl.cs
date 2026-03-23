
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModelLayer.DTO;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementModelLayer.Entities;
using QuantityMeasurementModelLayer.Enums;

namespace QuantityMeasurementBusinessLayer.Services;

public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
{
    private readonly IQuantityMeasurementRepositorySql repository;

    public QuantityMeasurementServiceImpl(IQuantityMeasurementRepositorySql repo)
    {
        repository = repo;
    }

    private double ConvertToBase(double value, string unit)
    {
        if (Enum.TryParse(unit, out LengthUnit length))   return value * length.GetConversionFactor();
        if (Enum.TryParse(unit, out WeightUnit weight))   return value * weight.GetConversionFactor();
        if (Enum.TryParse(unit, out VolumeUnit volume))   return value * volume.ToBaseUnit();
        if (Enum.TryParse(unit, out TemperatureUnit temp))
        {
            if (temp == TemperatureUnit.CELSIUS)    return value;
            if (temp == TemperatureUnit.FAHRENHEIT) return (value - 32) * 5 / 9;
        }
        throw new ArgumentException("Unsupported unit");
    }
 
    public bool Compare(QuantityDTO q1, QuantityDTO q2)
    {
        double v1 = ConvertToBase(q1.Value, q1.Unit);
        double v2 = ConvertToBase(q2.Value, q2.Unit);
        bool result = v1 == v2;
        var entity = new QuantityMeasurementEntity("COMPARE", q1.Value, q2.Value, result.ToString(), q1.Unit, q2.Unit);
        repository.Save(entity);
        return result;
    }
 
    public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
    {
        double v1 = ConvertToBase(q1.Value, q1.Unit);
        double v2 = ConvertToBase(q2.Value, q2.Unit);
        double value = v1 + v2;
        var entity = new QuantityMeasurementEntity("ADD", q1.Value, q2.Value, value.ToString(), q1.Unit, q2.Unit);
        repository.Save(entity);
        return new QuantityDTO(value, q1.Unit);
    }
 
    public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
    {
        double v1 = ConvertToBase(q1.Value, q1.Unit);
        double v2 = ConvertToBase(q2.Value, q2.Unit);
        double value = v1 - v2;
        var entity = new QuantityMeasurementEntity("SUBTRACT", q1.Value, q2.Value, value.ToString(), q1.Unit, q2.Unit);
        repository.Save(entity);
        return new QuantityDTO(value, q1.Unit);
    }
 
    public double Divide(QuantityDTO q1, QuantityDTO q2)
    {
        double v1 = ConvertToBase(q1.Value, q1.Unit);
        double v2 = ConvertToBase(q2.Value, q2.Unit);
        double result = v1 / v2;
        var entity = new QuantityMeasurementEntity("DIVIDE", q1.Value, q2.Value, result.ToString(), q1.Unit, q2.Unit);
        repository.Save(entity);
        return result;
    }
 
    public QuantityDTO Convert(QuantityDTO input, string targetUnit)
    {
        double baseValue = ConvertToBase(input.Value, input.Unit);
        var entity = new QuantityMeasurementEntity("CONVERSION", input.Value, 0, targetUnit);
        repository.Save(entity);
        
        if (Enum.TryParse(targetUnit, out LengthUnit length))
            return new QuantityDTO(baseValue / length.GetConversionFactor(), targetUnit);
        if (Enum.TryParse(targetUnit, out WeightUnit weight))
            return new QuantityDTO(baseValue / weight.GetConversionFactor(), targetUnit);
        if (Enum.TryParse(targetUnit, out VolumeUnit volume))
            return new QuantityDTO(baseValue / volume.ToBaseUnit(), targetUnit);
        if (Enum.TryParse(targetUnit, out TemperatureUnit temp))
        {
            if (temp == TemperatureUnit.CELSIUS)    return new QuantityDTO(baseValue, targetUnit);
            if (temp == TemperatureUnit.FAHRENHEIT) return new QuantityDTO(baseValue * 9 / 5 + 32, targetUnit);
        }
        throw new ArgumentException("Unsupported target unit");
    }
 
    public List<QuantityMeasurementEntity> GetAll()
    {
        return repository.GetAll();
    }
    public List<QuantityMeasurementEntity> GetCacheHistory() => repository.GetAll();
    public List<QuantityMeasurementEntity> GetRedisHistory() => repository.GetAll();
    public List<QuantityMeasurementEntity> GetSqlHistory() => repository.GetAll();
    public List<QuantityMeasurementEntity> GetEFHistory() => repository.GetAll(); 
}
