package toolgood.flowVision.Flows.Enums;

public enum InputType {
    Number(0),
    String(1),
    Bool(2),
    Date(3),
    List(4);
    private final int value;

    InputType(int v) {
        value = v;
    }

    public int getValue() {
        return value;
    }


    public static InputType intToEnum(int value) {
        switch (value) {
            case 0:
                return Number;
            case 1:
                return String;
            case 2:
                return Bool;
            case 3:
                return Date;
            case 4:
                return List;
            default:
                return null;
        }
    }
}
