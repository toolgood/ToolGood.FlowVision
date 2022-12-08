package toolgood.flowVision.Flows;

import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;

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

}
