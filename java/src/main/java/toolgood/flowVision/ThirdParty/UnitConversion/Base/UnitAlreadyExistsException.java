package toolgood.flowVision.ThirdParty.UnitConversion.Base;

public class UnitAlreadyExistsException extends Exception {
    public UnitAlreadyExistsException() {
    }

    public UnitAlreadyExistsException(String unit) {
        super("The given unit synonym '{" + unit + "}' is already used in this converter");
    }
}
