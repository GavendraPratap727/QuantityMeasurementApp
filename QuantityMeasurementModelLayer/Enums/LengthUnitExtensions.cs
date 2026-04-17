 using QuantityMeasurementModelLayer.Enums;
 namespace QuantityMeasurementModelLayer.Enums;
 public static class LengthUnitExtensions
    {
        public static double GetConversionFactor(this LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.FEET:
                case LengthUnit.FOOT: return 1.0;
                case LengthUnit.INCHES:
                case LengthUnit.INCH: return 1.0 / 12.0;
                case LengthUnit.YARDS:
                case LengthUnit.YARD: return 3.0;
                case LengthUnit.CENTIMETERS:
                case LengthUnit.CENTIMETER: return 1.0 / 30.48;
                case LengthUnit.METER: return 3.28084;
                case LengthUnit.KILOMETER:
                case LengthUnit.KILOMETRE: return 3280.84;
                case LengthUnit.MILE: return 5280.0;
                case LengthUnit.MILLIMETER:
                case LengthUnit.MILLIMETRE: return 1.0 / 304.8;
                default: throw new ArgumentException($"Unsupported length unit: {unit}");
            }
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString();
        }
    }
