package toolgood.flowVision.Flows;

import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;

import java.util.List;

public class CustomFlowWork extends NodeWork {
    public String CheckFormula;
    public String Script;
    public List<String> Names;

    public CustomFlowWork() {
        NodeType = CellType.Custom;
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
