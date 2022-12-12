package toolgood.flowVision.ThirdParty.UnitConversion.Base;

public abstract class BaseUnitConverter implements IUnitConverter {
    public String UnitLeft;
    public String UnitRight;
    protected UnitFactors Units;

    @Override
    public double LeftToRight(double value) throws UnitNotSupportedException {
        return AToB(value, UnitLeft, UnitRight);
    }

    @Override
    public double RightToLeft(double value) throws UnitNotSupportedException {
        return AToB(value, UnitRight, UnitLeft);
    }

    protected void Instantiate(UnitFactors conversionFactors) {
        Units = conversionFactors;
        UnitLeft = Units.BaseUnit;
        UnitRight = Units.BaseUnit;
    }

    protected void Instantiate(UnitFactors conversionFactors, String leftUnit, String rightUnit) {
        Units = conversionFactors;
        UnitLeft = leftUnit;
        UnitRight = rightUnit;
    }

    private double AToB(double value, String startUnit, String endUnit) throws UnitNotSupportedException {
        double startFactor = Units.FindFactor(startUnit);
        double endFactor = Units.FindFactor(endUnit);
        double result = (value / startFactor) * endFactor;
        return ExtensionMethods.CheckCloseEnoughValue(result);
    }


}
