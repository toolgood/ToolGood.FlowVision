package toolgood.algorithm.Enums;

public enum AreaUnitType {
    /// <summary>
    /// 平方毫米
    /// </summary>
    MM2(11),
    /// <summary>
    /// 平方厘米
    /// </summary>
    CM2(12),
    /// <summary>
    /// 平方分米
    /// </summary>
    DM2(13),
    /// <summary>
    /// 平方米
    /// </summary>
    M2(14),
    /// <summary>
    /// 平方千米
    /// </summary>
    KM2(15);

    private final int value;

    AreaUnitType(int v) {
        value = v;
    }

    public int getValue() {
        return value;
    }
}
