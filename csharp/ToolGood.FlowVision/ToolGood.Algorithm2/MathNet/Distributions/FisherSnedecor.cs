﻿using ToolGood.Algorithm2.MathNet.Numerics.RootFinding;

namespace ToolGood.Algorithm2.MathNet.Numerics.Distributions
{
	internal static class FisherSnedecor
	{
		public static double CDF(double d1, double d2, double x)
		{
			//if (d1 <= 0.0 || d2 <= 0.0) {
			//    throw new ArgumentException(Resources.InvalidDistributionParameters);
			//}

			return SpecialFunctions.BetaRegularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2));
		}

		public static double InvCDF(double d1, double d2, double p)
		{
			//if (d1 <= 0.0 || d2 <= 0.0) {
			//    throw new ArgumentException(Resources.InvalidDistributionParameters);
			//}

			return Brent.FindRoot(
				x => SpecialFunctions.BetaRegularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2)) - p,
				0, 1000, accuracy: 1e-12);
		}
	}
}