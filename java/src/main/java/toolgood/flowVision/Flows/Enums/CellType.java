package toolgood.flowVision.Flows.Enums;

public enum CellType {
    Edge(0),
    Start(1),
    End(2),
    Error(3),
    Procedure(4),
    Custom(5),
    Jump(6),
    Merge(7),
    Status(8);
    private final int value;

    CellType(int v) {
        value = v;
    }

    public static CellType intToEnum(int value) {
        switch (value) {
            case 0:
                return Edge;
            case 1:
                return Start;
            case 2:
                return End;
            case 3:
                return Error;
            case 4:
                return Procedure;
            case 5:
                return Custom;
            case 6:
                return Jump;
            case 7:
                return Merge;
            case 8:
                return Status;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
