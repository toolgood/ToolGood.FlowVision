using UnitConversion.Base;

namespace UnitConversion
{
    public sealed class TemperatureConverter: BaseUnitConverter
    {
        static UnitFactors units = new UnitFactors("celsius")
        {
            { new UnitFactorSynonyms("celsius", "Celsius", "°C", "°c","摄氏度","摄氏"),1 },
            { new UnitFactorSynonyms("fahrenheit", "Fahrenheit", "°F", "°f","华氏度","华氏"),1 },
            { new UnitFactorSynonyms("Kelvin", "kelvin", "°K", "°k","开尔文"),1 },
        };
        
        public TemperatureConverter(string leftUnit, string rightUnit)
        {
            Instantiate(units, leftUnit, rightUnit);
        }
        public TemperatureConverter() : base()
        {
            Instantiate(units);
        }
        public static bool Exists(string leftSynonym, string rightSynonym)
        {
            if (units.FindUnit(leftSynonym) != null) {
                return units.FindUnit(rightSynonym) != null;
            }
            return false;
        }
    }
}
