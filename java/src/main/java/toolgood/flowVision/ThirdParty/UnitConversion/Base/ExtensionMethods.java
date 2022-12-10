package toolgood.flowVision.ThirdParty.UnitConversion.Base;

public class ExtensionMethods {
    public static final double lowerBoundEpsilon = 0.99999;
    public static final double upperBoundEpsilon = 0.999999999;

    /// <summary>
    /// Return close enough value
    /// </summary>
    /// <param name="value">result value to be check</param>
    /// <returns>Closed value</returns>
    public static double CheckCloseEnoughValue(double value) {
        double diff = value - Math.floor(value);
        if (diff > lowerBoundEpsilon && diff < upperBoundEpsilon)
            return Math.ceil(value);
        return value;
    }

}
