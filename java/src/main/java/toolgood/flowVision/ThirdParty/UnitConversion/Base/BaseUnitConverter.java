package toolgood.flowVision.ThirdParty.UnitConversion.Base;

import java.math.BigDecimal;

public abstract class BaseUnitConverter implements IUnitConverter {
    public String UnitLeft;
    public String UnitRight;
    protected UnitFactors Units;

    @Override
    public BigDecimal LeftToRight(BigDecimal value) throws UnitNotSupportedException {
        return AToB(value, UnitLeft, UnitRight);
    }

    @Override
    public BigDecimal RightToLeft(BigDecimal value) throws UnitNotSupportedException {
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

    private BigDecimal AToB(BigDecimal value, String startUnit, String endUnit) throws UnitNotSupportedException {
        BigDecimal startFactor = Units.FindFactor(startUnit);
        BigDecimal endFactor = Units.FindFactor(endUnit);
        BigDecimal result = (value.divide(startFactor)).multiply(endFactor);
        return result;
    }


}
