package toolgood.flowVision.Flows;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;

import java.util.ArrayList;

public class ErrorFlowWork extends NodeWork {

    public ErrorFlowWork() {
        NodeType = toolgood.flowVision.Flows.Enums.CellType.Error;
    }
    public String CheckFormula;
    public String ErrorMessage;

    @Override
    public boolean Check(FlowEngine engine, String factoryCode) throws Exception {

        if (CheckFormula==null ||CheckFormula.equals("")) {
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

        result.CheckFormula = jsonObject.getString("checkFormula");
        result.ErrorMessage = jsonObject.getString("errorMessage");
        return result;
    }
}
