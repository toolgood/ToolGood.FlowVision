package toolgood.flowVision.ThirdParty.UnitConversion.Base;

public interface IUnitConverter {
    double LeftToRight(double value) throws UnitNotSupportedException;
    double RightToLeft(double value) throws UnitNotSupportedException;
}
