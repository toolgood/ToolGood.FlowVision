package toolgood.algorithm.Enums;

public enum VolumeUnitType {
    /// <summary>
    /// 立方毫米
    /// </summary>
    MM3(21),
    /// <summary>
    /// 立方厘米
    /// </summary>
    CM3(22),
    /// <summary>
    /// 立方分米
    /// </summary>
    DM3(23),
    /// <summary>
    /// 立方米
    /// </summary>
    M3(24),
    /// <summary>
    /// 立方千米
    /// </summary>
    KM3(25);

    private final int value;

    VolumeUnitType(int v) {
        value = v;
    }

    public int getValue() {
        return value;
    }
}
