using System;

namespace QuantityMeasurementModelLayer.Enums
{
    public static class VolumeUnitExtensions
    {
        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            switch (unit)
            {
                case VolumeUnit.LITRE:
                case VolumeUnit.LITER:
                    return value;

                case VolumeUnit.MILLILITRE:
                case VolumeUnit.MILLILITER:
                    return value * 0.001;

                case VolumeUnit.GALLON:
                    return value * 3.78541;

                case VolumeUnit.CUP:
                    return value * 0.236588;

                case VolumeUnit.TEASPOON:
                    return value * 0.00492892;

                case VolumeUnit.TABLESPOON:
                    return value * 0.0147868;

                default:
                    throw new ArgumentException($"Invalid Volume Unit: {unit}");
            }
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double value)
        {
            switch (unit)
            {
                case VolumeUnit.LITRE:
                case VolumeUnit.LITER:
                    return value;

                case VolumeUnit.MILLILITRE:
                case VolumeUnit.MILLILITER:
                    return value / 0.001;

                case VolumeUnit.GALLON:
                    return value / 3.78541;

                case VolumeUnit.CUP:
                    return value / 0.236588;

                case VolumeUnit.TEASPOON:
                    return value / 0.00492892;

                case VolumeUnit.TABLESPOON:
                    return value / 0.0147868;

                default:
                    throw new ArgumentException($"Invalid Volume Unit: {unit}");
            }
        }
    }
}