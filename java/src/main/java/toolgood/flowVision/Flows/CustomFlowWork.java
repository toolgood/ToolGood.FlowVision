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

public class CustomFlowWork extends NodeWork {
    public String CheckFormula;
    public String Script;
    public List<String> Names;

    public CustomFlowWork() {
        NodeType = CellType.Custom;
    }

    final static CustomFlowWork parse2(JSONObject jsonObject) {
        CustomFlowWork result = new CustomFlowWork();
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
        result.Script = jsonObject.getString("script");
        result.Names = new ArrayList<>();
        JSONArray array = jsonObject.getJSONArray("names");
        for (Object s : array) {
            result.Names.add(s.toString());
        }
        return result;
    }

    @Override
    public boolean Check(FlowEngine engine, String factoryCode) {
        if (CheckFormula == null || CheckFormula.equals("")) {
            return true;
        }
        mathParser.ProgContext progContext = null;
        try {
            progContext = Project.CreateProgContext(CheckFormula);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return engine.TryEvaluate(progContext, false);
    }
}
