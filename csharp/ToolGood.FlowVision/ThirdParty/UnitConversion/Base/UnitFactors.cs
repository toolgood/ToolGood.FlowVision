﻿//-----------------------------------------------------------------------
// <copyright file="UnitFactors.cs" company="George Kampolis">
//     Copyright (c) George Kampolis. All rights reserved.
//     Licensed under the MIT License, Version 2.0. See LICENSE.md in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------

namespace UnitConversion.Base
{
	using System.Collections.Generic;
	using System.Linq;

	public sealed class UnitFactors : Dictionary<UnitFactorSynonyms, double>
	{
		public UnitFactors(string baseUnit)
		{
			BaseUnit = baseUnit;
		}

		private string _baseUnit;

		public string BaseUnit {
			get
			{
				return _baseUnit;
			}
			private set
			{
				_baseUnit = value;
			}
		}

		// Find the key or null for a given unit
		internal UnitFactorSynonyms FindUnit(UnitFactorSynonyms synonyms)
		{
			return this.Keys.FirstOrDefault(factor => factor.Contains(synonyms));
		}

		// Get the factor for a given unit
		internal double FindFactor(UnitFactorSynonyms synonyms)
		{
			var unit = this.FirstOrDefault(factor => factor.Key.Contains(synonyms));
			if (unit.Key == null) {
				throw new UnitNotSupportedException(synonyms.ToString());
			}
			return unit.Value;
		}
	}
}