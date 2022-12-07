//-----------------------------------------------------------------------
// <copyright file="AreaConverter.cs" company="George Kampolis">
//     Copyright (c) George Kampolis. All rights reserved.
//     Licensed under the MIT License, Version 2.0. See LICENSE.md in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------

namespace UnitConversion
{
    using UnitConversion.Base;

    /// <summary>
    /// Converts between area units.
    /// </summary>
    public sealed class AreaConverter : BaseUnitConverter
    {
        static UnitFactors units = new UnitFactors("m²")
        {
            { new UnitFactorSynonyms("m²", "m2", "square metre", "centiare","平方米","平方公尺"), 1 },
            { new UnitFactorSynonyms("km²", "km2", "kilometre","平方千米"), 0.000001 },
            { new UnitFactorSynonyms("cm²", "cm2", "centimetre","平方厘米"), 10000 },
            { new UnitFactorSynonyms("mm²", "mm2", "millimetre","平方毫米"), 1000000 },
            { new UnitFactorSynonyms("ft²", "ft2", "square foot", "square feet", "sq ft","平方英尺"), 1d /  0.3048 /  0.3048 },
            { new UnitFactorSynonyms("yd²", "yd2", "sq yd", "square yard","平方码"), 1d /  0.9144 /  0.9144},
            { new UnitFactorSynonyms("a", "are"), 0.01 },
            { new UnitFactorSynonyms("ha", "hectare","公顷"), 0.0001 },
            { new UnitFactorSynonyms("in²", "in2", "sq in", "square inch","平方英寸"), 1d / 0.00064516 },
            { new UnitFactorSynonyms("mi²", "mi2", "sq mi", "square mile","平方英里"), 1d / 2589988.110336 },
            { new UnitFactorSynonyms( "亩"), 1d / 666.667 },
        };

        public AreaConverter(string leftUnit, string rightUnit)
        {
            Instantiate(units, leftUnit, rightUnit);
        }
        public AreaConverter()
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
