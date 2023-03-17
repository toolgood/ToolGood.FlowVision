package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONArray;
import com.alibaba.fastjson2.JSONObject;
import toolgood.algorithm2.Operand;
import toolgood.algorithm2.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.InputType;

import java.util.ArrayList;
import java.util.List;

public class SettingFormulaWork {
    public NodeWork NodeWork;
    public String Name;
    public InputType Type;
    public String DefaultFormula;
    public List<SettingFormulaItemWork> Conditions;
    private ProjectWork Project;

    final static SettingFormulaWork parse(JSONObject jsonObject) {
        SettingFormulaWork result = new SettingFormulaWork();
        result.Name = jsonObject.getString("name");
        result.DefaultFormula = jsonObject.getString("defaultFormula");
        result.Type = InputType.intToEnum(jsonObject.getIntValue("type"));

        result.Conditions = new ArrayList<>();
        JSONArray array2 = jsonObject.getJSONArray("conditions");
        for (Object s : array2) {
            if (s instanceof JSONObject) {
                JSONObject jsonObject1 = (JSONObject) s;
                SettingFormulaItemWork work = SettingFormulaItemWork.parse(jsonObject1);
                if (work != null) {
                    result.Conditions.add(work);
                }
            }
        }
        return result;
    }

    public Operand EvaluateFormula(FlowEngine engine) throws Exception {
        if (Conditions != null && Conditions.size() > 0) {
            for (int i = 0; i < Conditions.size(); i++) {
                SettingFormulaItemWork settingFormulaItem = Conditions.get(i);
                if (settingFormulaItem.Check(engine)) {
                    return settingFormulaItem.EvaluateFormula(engine, Type);
                }
            }
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(DefaultFormula);
        return engine.EvaluateFormula(progContext, Type);
    }

    public void Init(ProjectWork work) {
        Project = work;
        if (Conditions != null && Conditions.size() > 0) {
            for (int i = 0; i < Conditions.size(); i++) {
                SettingFormulaItemWork settingFormulaItem = Conditions.get(i);
                settingFormulaItem.Init(work);
            }
        }
    }
}
