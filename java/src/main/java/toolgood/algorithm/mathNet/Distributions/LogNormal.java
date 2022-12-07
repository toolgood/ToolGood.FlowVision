package toolgood.algorithm.mathNet.Distributions;

import toolgood.algorithm.mathNet.Constants;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class LogNormal {
    public static double CDF(double mu, double sigma, double x)
    {
        //if (sigma < 0.0) {
        //    throw new ArgumentException("InvalidDistributionParameters");
        //}

        return x < 0.0 ? 0.0
            : 0.5 * (1.0 + SpecialFunctions.Erf((Math.log(x) - mu) / (sigma * Constants.Sqrt2)));
    }

    public static double InvCDF(double mu, double sigma, double p)
    {
        //if (sigma < 0.0) {
        //    throw new ArgumentException("InvalidDistributionParameters");
        //}

        return p <= 0.0 ? 0.0 : p >= 1.0 ? Double.POSITIVE_INFINITY
            : Math.exp(mu - sigma * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2.0 * p));
    }
}