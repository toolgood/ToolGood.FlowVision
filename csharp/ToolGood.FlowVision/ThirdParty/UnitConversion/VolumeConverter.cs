//-----------------------------------------------------------------------
// <copyright file="VolumeConverter.cs" company="George Kampolis">
//     Copyright (c) George Kampolis. All rights reserved.
//     Licensed under the MIT License, Version 2.0. See LICENSE.md in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace UnitConversion
{
	using UnitConversion.Base;

	/// <summary>
	/// Converts between volume units.
	/// </summary>
	public sealed class VolumeConverter : BaseUnitConverter
	{
		private static UnitFactors units = new UnitFactors("l")
		{
			{ new UnitFactorSynonyms("l", "L", "lt", "ltr", "liter", "litre", "dm³", "dm3", "cubic decimetre","升","立方分米"), 1 },
			{ new UnitFactorSynonyms("m³", "m3", "cubic metre","立方米"), 0.001 },
			{ new UnitFactorSynonyms("cm³", "cm3", "cubic centimetre","立方厘米","毫升"), 1000 },
			{ new UnitFactorSynonyms("mm³", "mm3", "cubic millimetre","立方毫米"), 1000000 },
			{ new UnitFactorSynonyms("ft³", "ft3", "cubic foot", "cubic feet", "cu ft","立方英尺"), 0.0353147 },
			{ new UnitFactorSynonyms("in³", "in3", "cu in", "cubic inch","立方英寸"), 61.0237 },
			{ new UnitFactorSynonyms("imperial pint", "imperial pt", "imperial p"), 1.75975 },
			{ new UnitFactorSynonyms("imperial gallon", "imperial gal"), 0.219969 },
			{ new UnitFactorSynonyms("imperial quart", "imperial qt"), 0.879877 },
			{ new UnitFactorSynonyms("US pint", "US pt", "US p"), 2.11337643513819 },
			{ new UnitFactorSynonyms("US gallon", "US gal"), 0.264172 },
			{ new UnitFactorSynonyms("US quart", "US qt"), 2.11338 },
		};

		public VolumeConverter(string leftUnit, string rightUnit)
		{
			Instantiate(units, leftUnit, rightUnit);
		}

		public VolumeConverter()
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