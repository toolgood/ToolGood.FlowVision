package toolgood.algorithm2.internals;

import toolgood.algorithm2.AlgorithmEngine;
import toolgood.algorithm2.Operand;
import toolgood.algorithm2.litJson.JsonData;

public class AntlrLookupEngine extends AlgorithmEngine {
    public Operand Json;

    @Override
    protected Operand GetParameter(String parameter) {
        JsonData v = Json.JsonValue().GetChild(parameter);
        if (v != null) {
            if (v.IsString()) return Operand.Create(v.StringValue());
            if (v.IsBoolean()) return Operand.Create(v.BooleanValue());
            if (v.IsDouble()) return Operand.Create(v.NumberValue());
            if (v.IsObject()) return Operand.Create(v);
            if (v.IsArray()) return Operand.Create(v);
            if (v.IsNull()) return Operand.CreateNull();
            return Operand.Create(v);
        }
        return super.GetParameter(parameter);
    }
}