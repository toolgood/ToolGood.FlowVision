package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;

public class FactoryWork {
    private ProjectWork Project;// { get; set; }
    public String Code;
    public String Name;

    public void Init(ProjectWork work) {
        Project = work;
    }

    final static FactoryWork parse(JSONObject jsonObject) {
        FactoryWork result = new FactoryWork();
        result.Code = jsonObject.getString("code");
        result.Name = jsonObject.getString("name");

        return result;
    }
}
