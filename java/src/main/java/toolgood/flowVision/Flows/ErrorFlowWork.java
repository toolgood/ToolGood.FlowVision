package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONArray;
import com.alibaba.fastjson2.JSONObject;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ErrorFlowWork extends NodeWork {

    public ErrorFlowWork() {
        NodeType = toolgood.flowVision.Flows.Enums.CellType.Error;
    }

    public String CheckFormula;
    public String ErrorMessage;

    @Override
    public boolean Check(FlowEngine engine, String factoryCode) throws Exception {

        if (CheckFormula == null || CheckFormula.equals("")) {
            return true;
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(CheckFormula);
        return engine.TryEvaluate(progContext, false);
    }

    final static ErrorFlowWork parse2(JSONObject jsonObject) {
        ErrorFlowWork result = new ErrorFlowWork();
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
        result.ErrorMessage = jsonObject.getString("errorMessage");
        return result;
    }
}
