package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.regex.Pattern;

import org.joda.time.DateTime;

import toolgood.algorithm.Enums.OperandType;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.litJson.JsonData;

public abstract class Operand {
    public final static Operand True = new OperandBoolean(true);
    public final static Operand False = new OperandBoolean(false);
    public final static Operand One = Operand.Create(1);
    public final static Operand Zero = Operand.Create(0);

    public boolean IsNull() {
        return false;
    }

    public boolean IsError() {
        return false;
    }

    public String ErrorMsg() {
        return null;
    }

    public OperandType Type() {
        return OperandType.ERROR;
    }

    public double NumberValue() {
        return 0.0;
    }

    public int IntValue() {
        return 0;
    }

    public String TextValue() {
        return null;
    }

    public boolean BooleanValue() {
        return false;
    }

    public List<Operand> ArrayValue() {
        return null;
    }

    public JsonData JsonValue() {
        return null;
    }

    public MyDate DateValue() {
        return null;
    }

    public static Operand Create(final boolean obj) {
        return obj ? True : False;
    }

    public static Operand Create(final short obj) {
        return new OperandNumber((double) obj);
    }

    public static Operand Create(final int obj) {
        return new OperandNumber((double) obj);
    }

    public static Operand Create(final long obj) {
        return new OperandNumber((double) obj);
    }

    public static Operand Create(final float obj) {
        return new OperandNumber((double) obj);
    }

    public static Operand Create(final double obj) {
        return new OperandNumber(obj);
    }

    public static Operand Create(final BigDecimal obj) {
        return new OperandNumber(obj.doubleValue());
    }

    public static Operand Create(final String obj) {
        if (obj.equals(null)) {
            return CreateNull();
        }
        return new OperandString(obj);
    }

    public static Operand CreateJson(final String txt) {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = (JsonData) JsonMapper.ToObject(txt);
                return Create(json);
            } catch (final Exception e) {
            }
        }
        return Error("string to json is error!");
    }

    public static Operand Create(final MyDate obj) {
        return new OperandDate(obj);
    }

    public static Operand Create(final DateTime obj) {
        return new OperandDate(new MyDate(obj));
    }

    public static Operand Create(final Date obj) {
        return new OperandDate(new MyDate(obj));
    }

    public static Operand Create(final JsonData obj) {
        return new OperandJson(obj);
    }

    public static Operand Create(List<Operand> obj) {
        return new OperandArray(obj);
    }

    public static Operand Error(final String msg) {
        return new OperandError(msg);
    }

    public static Operand CreateNull() {
        return new OperandNull();
    }

    public Operand ToNumber() {
        return ToNumber(null);
    }

    public Operand ToNumber(final String errorMessage) {
        if (Type() == OperandType.NUMBER) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.BOOLEAN) {
            return BooleanValue() ? One : Zero;
        }
        if (Type() == OperandType.DATE) {
            return Create((double) DateValue().ToNumber());
        }
        if (Type() == OperandType.TEXT) {
            try {
                Double d = Double.parseDouble(TextValue());
                return Create(d);
            } catch (Exception e) {
            }
        }
        return Error(errorMessage);
    }

    public Operand ToBoolean() {
        return ToBoolean(null);
    }

    public Operand ToBoolean(final String errorMessage) {
        if (Type() == OperandType.BOOLEAN) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.NUMBER) {
            return (NumberValue() != 0) ? True : False;
        }
        if (Type() == OperandType.DATE) {
            return (((double) DateValue().ToNumber()) != 0) ? True : False;
        }
        if (Type() == OperandType.TEXT) {
            if (TextValue().toLowerCase().equals("true")) {
                return True;
            }
            if (TextValue().toLowerCase().equals("false")) {
                return False;
            }
            if (TextValue().equals("1")) {
                return True;
            }
            if (TextValue().equals("0")) {
                return False;
            }
        }
        return Error(errorMessage);
    }

    public Operand ToText() {
        return ToText(null);
    }

    public Operand ToText(final String errorMessage) {
        if (Type() == OperandType.TEXT) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.NUMBER) {
            String str = ((Double) NumberValue()).toString();
            if (str.contains(".")) {
                str = Pattern.compile("(\\.)?0+$").matcher(str).replaceAll("");
            }
            return Create(str);
        }
        if (Type() == OperandType.BOOLEAN) {
            return Create(BooleanValue() ? "TRUE" : "FALSE");
        }
        if (Type() == OperandType.DATE) {
            return Create(DateValue().toString());
        }

        return Error(errorMessage);
    }

    public Operand ToDate() {
        return ToDate(null);
    }

    public Operand ToDate(final String errorMessage) {
        if (Type() == OperandType.DATE) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.NUMBER) {
            return Create(new MyDate(NumberValue()));
        }
        if (Type() == OperandType.TEXT) {
            MyDate date = MyDate.parse(TextValue());
            if (date != null) {
                return Create(date);
            }
            // if (TimeSpan.TryParse(TextValue, cultureInfo, out TimeSpan t)) { return
            // Create(new Date(t)); }
            // if (DateTime.TryParse(TextValue, cultureInfo, DateTimeStyles.None, out
            // DateTime d)) { return Create(new Date(d)); }
        }
        return Error(errorMessage);
    }

    public Operand ToJson() {
        return ToJson(null);
    }

    public Operand ToJson(final String errorMessage) {
        if (Type() == OperandType.JSON) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.TEXT) {
            final String txt = TextValue();
            if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
                try {
                    final JsonData json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (final Exception e) {
                }
            }
        }
        return Error(errorMessage);
    }

    public Operand ToArray() {
        return ToArray(null);
    }

    public Operand ToArray(final String errorMessage) {
        if (Type() == OperandType.ARRARY) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.JSON) {
            if (JsonValue().IsArray()) {
                final List<Operand> list = new ArrayList<Operand>();
                for (JsonData v : JsonValue().inst_array) {
                    if (v.IsString())
                        list.add(Create(v.StringValue()));
                    else if (v.IsBoolean())
                        list.add(Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        list.add(Create(v.NumberValue()));
                    else if (v.IsNull())
                        list.add(CreateNull());
                    else
                        list.add(Create(v));
                }
                return Create(list);
            }
        }
        return Error(errorMessage);
    }

    static abstract class OperandT<T> extends Operand {
        public T Value;

        public OperandT(final T obj) {
            Value = obj;
        }
    }

    static class OperandArray extends OperandT<java.util.List<Operand>> {
        public OperandArray(final List<Operand> obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.ARRARY;
        }

        @Override
        public List<Operand> ArrayValue() {
            return Value;
        }
    }

    static class OperandBoolean extends OperandT<Boolean> {
        public OperandBoolean(final Boolean obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.BOOLEAN;
        }

        @Override
        public boolean BooleanValue() {
            return Value;
        }
    }

    static class OperandDate extends OperandT<MyDate> {
        public OperandDate(final MyDate obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.DATE;
        }

        @Override
        public MyDate DateValue() {
            return Value;
        }
    }

    static class OperandError extends Operand {
        private final String _errorMsg;

        public OperandError(final String msg) {
            _errorMsg = msg;
        }

        @Override
        public OperandType Type() {
            return OperandType.ERROR;
        }

        @Override
        public boolean IsError() {
            return true;
        }

        public String ErrorMsg() {
            return _errorMsg;
        }
    }

    static class OperandJson extends OperandT<JsonData> {
        public OperandJson(final JsonData obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.JSON;
        }

        @Override
        public JsonData JsonValue() {
            return Value;
        }
    }

    static class OperandNull extends Operand {
        // public override OperandType Type => OperandType.NULL;
        // public override bool IsNull => true;

        @Override
        public OperandType Type() {
            return OperandType.NULL;
        }

        @Override
        public boolean IsNull() {
            return true;
        }


        @Override
        public Operand ToArray(String errorMessage) {
            return Error(errorMessage != null ? errorMessage : "Convert null to array error!");
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            return Error(errorMessage != null ? errorMessage : "Convert null to bool error!");
        }

        @Override
        public Operand ToText(String errorMessage) {
            return Error(errorMessage != null ? errorMessage : "Convert null to string error!");
        }

        @Override
        public Operand ToNumber(String errorMessage) {
            return Error(errorMessage != null ? errorMessage : "Convert null to number error!");
        }

        @Override
        public Operand ToJson(String errorMessage) {
            return Error(errorMessage != null ? errorMessage : "Convert null to json error!");
        }

        @Override
        public Operand ToDate(String errorMessage) {
            return Error(errorMessage != null ? errorMessage : "Convert null to date error!");
        }
    }

    static class OperandNumber extends OperandT<Double> {

        public OperandNumber(Double obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.NUMBER;
        }

        @Override
        public int IntValue() {
            return (int) (double) Value;
        }

        @Override
        public double NumberValue() {
            return Value;
        }

    }

    static class OperandString extends OperandT<String> {

        public OperandString(String obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.TEXT;
        }

        @Override
        public String TextValue() {
            return Value;
        }

    }

    public static class KeyValue {
        public Integer Index;
        public String Key;
        public Operand Value;
    }

    public static class OperandKeyValue extends OperandT<KeyValue> {
        public OperandKeyValue(KeyValue obj) {
            super(obj);
        }

        public OperandType Type() {
            return OperandType.ARRARYJSON;
        }
    }

    public static class OperandKeyValueList extends OperandT<KeyValue> {
        public OperandKeyValueList(KeyValue obj, int excelIndex) {
            super(obj);
            listNum = excelIndex;
        }

        public OperandType Type() {
            return OperandType.ARRARYJSON;
        }

        public List<Operand> ArrayValue() {
            List<Operand> result = new ArrayList<>();
            for (KeyValue kv : TextList) {
                result.add(kv.Value);
            }
            return result;
        }

        private int listNum;
        private List<KeyValue> TextList = new ArrayList<>();

        public Operand ToArray(String errorMessage) {
            return Create(this.ArrayValue());
        }

        public void AddValue(KeyValue keyValue) {
            if (keyValue.Index != null) {
                listNum = keyValue.Index;
                keyValue.Key = keyValue.Index.toString();
            } else {
                keyValue.Index = listNum;
            }
            TextList.add(keyValue);
            listNum++;
        }

        public boolean HasKey(int key) {
            for (var item : TextList) {
                if (item.Index == key) {
                    return true;
                }
            }
            for (var item : TextList) {
                if (item.Key.equals("" + key)) {
                    return true;
                }
            }
            return false;
        }

        public Operand GetValue(int key) {
            for (var item : TextList) {
                if (item.Index == key) {
                    return item.Value;
                }
            }
            for (var item : TextList) {
                if (item.Key.equals("" + key)) {
                    return item.Value;
                }
            }
            return null;
        }
        public boolean HasKey(String key) {
            for (var item : TextList) {
                if (item.Key.equals("" + key)) {
                    return true;
                }
            }
            return false;
        }
        public Operand GetValue(String key) {
            for (var item : TextList) {
                if (item.Key.equals( key)) {
                    return item.Value;
                }
            }
            return null;
        }


        public boolean ContainsValue(int value) {
            for (var item : TextList) {
                Operand op = item.Value;
                if (op.TextValue().equals(value + "")) {
                    return true;
                }
            }
            return false;
        }

        public boolean ContainsValue(String value) {
            for (var item : TextList) {
                Operand op = item.Value;
                if (op.TextValue().equals(value)) {
                    return true;
                }
            }
            return false;
        }

        public boolean ContainsValue(Operand value) {
            for (var item : TextList) {
                Operand op = item.Value;
                if (value.Type() != op.Type()) {
                    continue;
                }
                if (value.Type() == OperandType.TEXT) {
                    if (value.TextValue().equals(op.TextValue())) {
                        return true;
                    }
                }
                if (value.Type() == OperandType.NUMBER) {
                    if (value.TextValue().equals(op.TextValue())) {
                        return true;
                    }
                }
            }
            return false;
        }

        public Operand TryGetValueFloor(double key, boolean range_lookup ) {
            Operand value = null;
            for (var item : TextList) {
                try {
                    double num = Double.parseDouble(item.Key);
                    Double t = Math.round(key - num * 1000000000d) / 1000000000d;
                    if (t == 0) {
                        return item.Value;
                    } else if (range_lookup) {
                        if (t > 0) {
                            value = item.Value;
                        } else if (value != null) {
                            return value;
                        }
                    }
                } catch (Exception ex) {
                }
            }
            return value  ;
        }
    }


}
