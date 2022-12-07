﻿//-----------------------------------------------------------------------
// <copyright file="TimeConverter.cs" company="George Kampolis">
//     Copyright (c) George Kampolis. All rights reserved.
//     Licensed under the MIT License, Version 2.0. See LICENSE.md in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------

namespace UnitConversion
{
    using UnitConversion.Base;

    /// <summary>
    /// Converts between time units.
    /// </summary>
    public sealed class TimeConverter : BaseUnitConverter
    {
        static UnitFactors units = new UnitFactors("s")
        {
            { new UnitFactorSynonyms("s", "sec", "second","秒"), 1 },
            { new UnitFactorSynonyms("ds", "decisecond", "deci-second"), 10 },
            { new UnitFactorSynonyms("cs", "centisecond", "centi-second"), 100 },
            { new UnitFactorSynonyms("ms", "millisecond", "milli-second"), 1_000 },
            { new UnitFactorSynonyms("us", "μs",  "microsecond", "micro-second"), 1_000_000 },
            { new UnitFactorSynonyms("ns", "nanosecond", "nano-second"), 1_000_000_000 },

            { new UnitFactorSynonyms("min", "minute","分"), 1d / 60 },
            { new UnitFactorSynonyms("h", "hour","时"), 1d / 3600 },
            { new UnitFactorSynonyms("d", "day","天"), 1d / 86400 },
            { new UnitFactorSynonyms("y", "year","年"), 1d / 31536000 },
            { new UnitFactorSynonyms("leap year", "leap-year"), 1d / 31622400 },
        };

        public TimeConverter(string leftUnit, string rightUnit)
        {
            Instantiate(units, leftUnit, rightUnit);
        }
        public TimeConverter()
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
