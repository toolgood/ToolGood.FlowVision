package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONArray;
import com.alibaba.fastjson2.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class AppInitValueWork {
    public String Name;
    public toolgood.flowVision.Flows.Enums.InputType InputType;
    public List<SettingFormulaItemWork> Conditions;

    public boolean IsThrowError;
    public String ErrorMessage;

    final static AppInitValueWork parse(JSONObject jsonObject) {
        AppInitValueWork result = new AppInitValueWork();
        result.Name = jsonObject.getString("name");
        result.InputType = toolgood.flowVision.Flows.Enums.InputType.intToEnum(jsonObject.getIntValue("inputType"));
        result.Conditions = new ArrayList<>();
        JSONArray conditions = jsonObject.getJSONArray("conditions");
        for (Object item : conditions) {
            if (item instanceof JSONObject) {
                JSONObject jsonObject1 = (JSONObject) item;
                SettingFormulaItemWork itemWork = SettingFormulaItemWork.parse(jsonObject1);
                if (itemWork != null) {
                    result.Conditions.add(itemWork);
                }
            }
        }
        return result;
    }

    public void Init(ProjectWork work) {
        for (int i = 0; i < Conditions.size(); i++) {
            SettingFormulaItemWork item = Conditions.get(i);
            item.Init(work);
        }
    }
}
