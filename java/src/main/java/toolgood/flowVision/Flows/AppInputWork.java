package toolgood.flowVision.Flows;

import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.InputType;

public class AppInputWork {
    private ProjectWork Project;

    public String InputName;
    public String Unit;
    public toolgood.flowVision.Flows.Enums.InputType InputType;
    public String CheckInput;
    public boolean IsRequired;
    public boolean UseDefaultValue;
    public String DefaultValue;
    public String ErrorMessage;
    public String Comment;

    public boolean Check(FlowEngine engine) throws Exception {
        if (CheckInput == null || CheckInput.equals("")) {
            return true;
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(CheckInput);
        Operand operand = engine.EvaluateFormula(progContext, InputType.Bool);
        return operand.BooleanValue();
    }

    public void Init(ProjectWork work) {
        Project = work;
    }

}
