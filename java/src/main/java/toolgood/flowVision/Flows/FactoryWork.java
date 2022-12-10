package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;

public class FactoryWork {
    public String Code;
    public String Name;

    public void Init(ProjectWork work) {
    }

    final static FactoryWork parse(JSONObject jsonObject) {
        FactoryWork result = new FactoryWork();
        result.Code = jsonObject.getString("code");
        result.Name = jsonObject.getString("name");

        return result;
    }
}
