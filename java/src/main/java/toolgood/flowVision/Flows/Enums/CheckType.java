package toolgood.flowVision.Flows.Enums;

public enum CheckType {
    Add(0),
    Replace(1);
    private final int value;

    CheckType(int v) {
        value = v;
    }

    public static CheckType intToEnum(int value) {
        switch (value) {
            case 0:
                return Add;
            case 1:
                return Replace;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
