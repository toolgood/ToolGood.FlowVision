package toolgood.algorithm.mathNet.Distributions;

import java.util.function.Function;

import toolgood.algorithm.mathNet.SpecialFunctions;
import toolgood.algorithm.mathNet.RootFinding.Brent;

public class StudentT {
    public static double CDF(double location, double scale, double freedom, double x)
    {
        //if (scale <= 0.0 || freedom <= 0.0) {
        //    throw new ArgumentException(Resources.InvalidDistributionParameters);
        //}

        if (Double.isInfinite(freedom)) {
            return Normal.CDF(location, scale, x);
        }

        double k = (x - location) / scale;
        double h = freedom / (freedom + (k * k));
        double ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2.0, 0.5, h);
        return x <= location ? ib : 1.0 - ib;
    }
    public static double InvCDF(double location, double scale, double freedom, double p) throws Exception
    {
        //if (scale <= 0.0 || freedom <= 0.0) {
        //    throw new ArgumentException(Resources.InvalidDistributionParameters);
        //}

        if (Double.isInfinite(freedom)) {
            return Normal.InvCDF(location, scale, p);
        }

        if (p == 0.5d) {
            return location;
        }
        Function<Double,Double> f=x->{
            double k = (x - location) / scale;
            double h = freedom / (freedom + (k * k));
            double ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2.0, 0.5, h);
            return x <= location ? ib - p : 1.0 - ib - p;
        };

        return Brent.FindRoot(f, -800, 800,  1e-12);
    }

}