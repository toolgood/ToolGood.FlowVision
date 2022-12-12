package toolgood.flowVision.Flows.Enums;

public enum InputNumberType {
    Original(0),
    Ceil(1),
    Floor(2),
    Round0(3),
    Round1(4),
    Round2(5),
    Round3(6),
    Round4(7),
    Round5(8),
    Round6(9),
    Round7(10),
    Round8(11);
    private final int value;

    InputNumberType(int v) {
        value = v;
    }

    public static InputNumberType intToEnum(int value) {
        switch (value) {
            case 0:
                return Original;
            case 1:
                return Ceil;
            case 2:
                return Floor;
            case 3:
                return Round0;
            case 4:
                return Round1;
            case 5:
                return Round2;
            case 6:
                return Round3;
            case 7:
                return Round4;
            case 8:
                return Round5;
            case 9:
                return Round6;
            case 10:
                return Round7;
            case 11:
                return Round8;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
