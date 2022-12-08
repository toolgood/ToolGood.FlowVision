package toolgood.flowVision.Flows;

import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.InputType;

public class SettingFormulaItemWork {
    private ProjectWork Project;// { get; set; }

    public String Condition;
    public String Formula;


    public Operand EvaluateFormula(FlowEngine engine, InputType inputType) throws Exception {
        mathParser.ProgContext progContext = Project.CreateProgContext(Formula);
        return engine.EvaluateFormula(progContext, inputType);
    }

    public void Init(ProjectWork work) {
        Project = work;
    }


    public boolean Check(FlowEngine engine) throws Exception {
        if (Condition == null || Condition.equals("")) {
            return true;
        }
        if (Condition == "1" || Condition.equalsIgnoreCase("true")) {
            return true;
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(Condition);
        return engine.TryEvaluate(progContext, false);
    }
}
