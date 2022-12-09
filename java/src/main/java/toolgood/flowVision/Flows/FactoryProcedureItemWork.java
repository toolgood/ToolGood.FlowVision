package toolgood.flowVision.Flows;

import com.alibaba.fastjson.JSONObject;

public class FactoryProcedureItemWork {
    public String CategoryCode;
    public String Category;
    public String Code;
    public String Name;

    final static FactoryProcedureItemWork parse(JSONObject jsonObject) {
        FactoryProcedureItemWork result = new FactoryProcedureItemWork();
        result.CategoryCode = jsonObject.getString("categoryCode");
        result.Category = jsonObject.getString("category");
        result.Code = jsonObject.getString("code");
        result.Name = jsonObject.getString("name");

        return result;
    }
}
