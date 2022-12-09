package toolgood.flowVision.Flows;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Interfaces.ISettingFormulaNodeWork;

import java.util.ArrayList;
import java.util.List;

public class MergeFlowWork extends NodeWork implements ISettingFormulaNodeWork {
    public List<SettingFormulaWork> SettingFormula;

    public MergeFlowWork() {
        NodeType = CellType.Merge;
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

    final static MergeFlowWork parse2(JSONObject jsonObject) {
        MergeFlowWork result = new MergeFlowWork();
        result.Id = jsonObject.getString("id");
        result.Label = jsonObject.getString("label");
        result.Layer = jsonObject.getIntValue("layer");
        result.NodeType = CellType.intToEnum(jsonObject.getIntValue("nodeType"));

        result.SettingFormula =new ArrayList<>();
        JSONArray array = jsonObject.getJSONArray("settingFormula");
        for (Object s : array) {
            if (s instanceof JSONObject jsonObject1){
                SettingFormulaWork work=SettingFormulaWork.parse(jsonObject1);
                if (work!=null){
                    result.SettingFormula.add(work);
                }
            }
        }
        return result;
    }
}
