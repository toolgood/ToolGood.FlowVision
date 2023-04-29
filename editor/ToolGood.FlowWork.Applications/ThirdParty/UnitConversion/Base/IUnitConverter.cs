//-----------------------------------------------------------------------
// <copyright file="IUnitConverter.cs" company="George Kampolis">
//     Copyright (c) George Kampolis. All rights reserved.
//     Licensed under the MIT License, Version 2.0. See LICENSE.md in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------

namespace UnitConversion.Base
{
	internal interface IUnitConverter
	{
        decimal LeftToRight(decimal value);

        decimal RightToLeft(decimal value);
	}
}