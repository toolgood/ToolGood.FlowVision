package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONArray;
import com.alibaba.fastjson2.JSONObject;
import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Enums.InputType;
import toolgood.flowVision.Flows.Interfaces.ISettingFormulaNodeWork;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class StatusFlowWork extends NodeWork implements ISettingFormulaNodeWork {

    public List<SettingFormulaWork> SettingFormula;
    public String CheckFormula;
    public String Status;
    public String StatusCheckFormula;

    public StatusFlowWork() {
        NodeType = CellType.Status;
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
    public boolean Check(FlowEngine engine, String factoryCode) throws Exception {
        if (CheckFormula == null || CheckFormula.equals("")) {
            return true;
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(CheckFormula);
        return engine.TryEvaluate(progContext, false);
    }

    public boolean CheckStatus(FlowEngine engine) throws Exception {
        if (CheckFormula == null || CheckFormula.equals("")) {
            return true;
        }
        var progContext = Project.CreateProgContext(StatusCheckFormula);
        Operand operand = engine.EvaluateFormula(progContext, InputType.Bool);
        return operand.BooleanValue();
    }


    @Override
    public List<SettingFormulaWork> SettingFormula() {
        return SettingFormula;
    }


    final static StatusFlowWork parse2(JSONObject jsonObject) {
        StatusFlowWork result = new StatusFlowWork();
        result.Id = jsonObject.getString("id");
        result.Label = jsonObject.getString("label");
        result.Layer = jsonObject.getIntValue("layer");
        result.NodeType = CellType.intToEnum(jsonObject.getIntValue("nodeType"));
        result.NextNodeIds = new HashMap<>();
        if (jsonObject.containsKey("nextNodeIds")) {
            JSONObject nextNodeIds = jsonObject.getJSONObject("nextNodeIds");
            for (Map.Entry<String, Object> kv : nextNodeIds.entrySet()) {
                if (kv.getValue() instanceof JSONArray array) {
                    List<String> list = new ArrayList<>();
                    for (Object obj : array) {
                        list.add(obj.toString());
                    }
                    result.NextNodeIds.put(kv.getKey(), list);
                }
            }
        }

        result.CheckFormula = jsonObject.getString("checkFormula");
        result.Status = jsonObject.getString("status");
        result.StatusCheckFormula = jsonObject.getString("statusCheckFormula");

        result.SettingFormula = new ArrayList<>();
        JSONArray array = jsonObject.getJSONArray("settingFormula");
        for (Object s : array) {
            if (s instanceof JSONObject jsonObject1) {
                SettingFormulaWork work = SettingFormulaWork.parse(jsonObject1);
                if (work != null) {
                    result.SettingFormula.add(work);
                }
            }
        }
        return result;
    }
}
