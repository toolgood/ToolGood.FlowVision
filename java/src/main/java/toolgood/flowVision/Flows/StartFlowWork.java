package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONArray;
import com.alibaba.fastjson2.JSONObject;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Interfaces.ISettingFormulaNodeWork;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class StartFlowWork extends NodeWork implements ISettingFormulaNodeWork {

    public List<SettingFormulaWork> SettingFormula;

    public StartFlowWork() {
        NodeType = CellType.Start;
    }

    final static StartFlowWork parse2(JSONObject jsonObject) {
        StartFlowWork result = new StartFlowWork();
        result.Id = jsonObject.getString("id");
        result.Label = jsonObject.getString("label");
        result.Layer = jsonObject.getIntValue("layer");
        result.NodeType = CellType.intToEnum(jsonObject.getIntValue("nodeType"));
        result.NextNodeIds = new HashMap<>();
        if (jsonObject.containsKey("nextNodeIds")) {
            JSONObject nextNodeIds = jsonObject.getJSONObject("nextNodeIds");
            for (Map.Entry<String, Object> kv : nextNodeIds.entrySet()) {
                if (kv.getValue() instanceof JSONArray) {
                    List<String> list = new ArrayList<>();
                    JSONArray array = (JSONArray) kv.getValue();
                    for (Object obj : array) {
                        list.add(obj.toString());
                    }
                    result.NextNodeIds.put(kv.getKey(), list);
                }
            }
        }


        result.SettingFormula = new ArrayList<>();
        JSONArray array = jsonObject.getJSONArray("settingFormula");
        for (Object s : array) {
            if (s instanceof JSONObject) {
                JSONObject jsonObject1 = (JSONObject) s;
                SettingFormulaWork work = SettingFormulaWork.parse(jsonObject1);
                if (work != null) {
                    result.SettingFormula.add(work);
                }
            }
        }
        return result;
    }

    @Override
    public void Init(ProjectWork work, AppWork app) {
        super.Init(work, app);
        for (int i = 0; i < SettingFormula.size(); i++) {
            SettingFormulaWork item = SettingFormula.get(i);
            item.Init(work);
            item.NodeWork = this;
        }
    }

    @Override
    public List<SettingFormulaWork> SettingFormula() {
        return SettingFormula;
    }
}
