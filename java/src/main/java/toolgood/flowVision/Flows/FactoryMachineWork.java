package toolgood.flowVision.Flows;

import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;

public class FactoryMachineWork {
    private ProjectWork Project;

    public String Factory;
    public String MachineCode;
    public String MachineName;
    public String MachineCategoryCode;
    public String MachineCategoryName;
    public String CheckFormula;

    public void Init(ProjectWork work) {
        Project = work;
    }

    public boolean Check(FlowEngine engine) throws Exception {
        if (CheckFormula == null || CheckFormula.equals("")) {
            return true;
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(CheckFormula);
        return engine.TryEvaluate(progContext, false);
    }
}
