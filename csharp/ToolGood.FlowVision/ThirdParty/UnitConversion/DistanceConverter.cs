//-----------------------------------------------------------------------
// <copyright file="DistanceConverter.cs" company="George Kampolis">
//     Copyright (c) George Kampolis. All rights reserved.
//     Licensed under the MIT License, Version 2.0. See LICENSE.md in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------


namespace UnitConversion
{
    using UnitConversion.Base;

    /// <summary>
    /// Converts between distance units.
    /// </summary>
    public sealed class DistanceConverter : BaseUnitConverter
    {
        static UnitFactors units = new UnitFactors("m")
        {
            { new UnitFactorSynonyms("m", "metre","米"), 1 },
            { new UnitFactorSynonyms("km", "kilometre","千米"), 0.001 },
            { new UnitFactorSynonyms("dm","decimetre", "分米"), 10 },
            { new UnitFactorSynonyms("cm", "centimetre", "厘米"), 100 },
            { new UnitFactorSynonyms("mm", "millimetre", "毫米"), 1000 },
            { new UnitFactorSynonyms("ft", "foot", "feet","英尺"), 1250d / 381 },
            { new UnitFactorSynonyms("yd", "yard","码"), 1250d / 1143 },
            { new UnitFactorSynonyms("mile","英里"), 125d / 201168 },
            { new UnitFactorSynonyms("in", "inch","英寸"), 5000d / 127 },
            { "au", 1d / 149600000000}
        };

        public DistanceConverter(string leftUnit, string rightUnit)
        {
            Instantiate(units, leftUnit, rightUnit);
        }
        public DistanceConverter()
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
