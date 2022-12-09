package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;

public class FactoryWork {
    private ProjectWork Project;// { get; set; }
    public String Category;
    public String Code;
    public String Name;
    public String SimplifyName;

    public void Init(ProjectWork work) {
        Project = work;
    }

    final static FactoryWork parse(JSONObject jsonObject) {
        FactoryWork result = new FactoryWork();
        result.Category = jsonObject.getString("category");
        result.Code = jsonObject.getString("code");
        result.Name = jsonObject.getString("name");
        result.SimplifyName = jsonObject.getString("simplifyName");

        return result;
    }
}
