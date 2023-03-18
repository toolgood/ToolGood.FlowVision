package toolgood.flowVision.ThirdParty.UnitConversion.Base;

import java.math.BigDecimal;

public interface IUnitConverter {
    BigDecimal LeftToRight(BigDecimal value) throws UnitNotSupportedException;

    BigDecimal RightToLeft(BigDecimal value) throws UnitNotSupportedException;
}
