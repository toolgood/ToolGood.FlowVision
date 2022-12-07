package toolgood.algorithm.mathNet.Distributions;

import java.util.function.Function;

import toolgood.algorithm.mathNet.SpecialFunctions;
import toolgood.algorithm.mathNet.RootFinding.Brent;

public class Beta {
    
    public static double CDF(double a, double b, double x)
    {
        //if (a < 0.0 || b < 0.0) {
        //    throw new ArgumentException(Resources.InvalidDistributionParameters);
        //}

        if (x < 0.0) {
            return 0.0;
        }

        if (x >= 1.0) {
            return 1.0;
        }
        
        if (Double.isInfinite(a) && Double.isInfinite(b)) {
            return x < 0.5 ? 0.0 : 1.0;
        }

        if (Double.isInfinite(a)) {
            return x < 1.0 ? 0.0 : 1.0;
        }

        if (Double.isInfinite(b)) {
            return x >= 0.0 ? 1.0 : 0.0;
        }

        if (a == 0.0 && b == 0.0) {
            if (x >= 0.0 && x < 1.0) {
                return 0.5;
            }

            return 1.0;
        }

        if (a == 0.0) {
            return 1.0;
        }

        if (b == 0.0) {
            return x >= 1.0 ? 1.0 : 0.0;
        }

        if (a == 1.0 && b == 1.0) {
            return x;
        }

        return SpecialFunctions.BetaRegularized(a, b, x);
    }

    public static double InvCDF(double a, double b, double p) throws Exception
    {

        //if (a < 0.0 || b < 0.0 || p < 0.0 || p > 1.0) {
        //    throw new ArgumentException(Resources.InvalidDistributionParameters);
        //}
        Function<Double,Double> f= x->{
            return SpecialFunctions.BetaRegularized(a, b, x) - p;
        };

        return Brent.FindRoot(f, 0.0, 1.0,   1e-12);
    }
 
}