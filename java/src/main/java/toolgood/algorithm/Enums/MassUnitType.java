package toolgood.algorithm.Enums;

public enum MassUnitType {
    /// <summary>
    /// 克
    /// </summary>
    G(31),
    /// <summary>
    /// 千克
    /// </summary>
    KG(32),
    /// <summary>
    /// 吨
    /// </summary>
    T(33);

    private final int value;

    MassUnitType(int v) {
        value = v;
    }

    public int getValue() {
        return value;
    }
}
