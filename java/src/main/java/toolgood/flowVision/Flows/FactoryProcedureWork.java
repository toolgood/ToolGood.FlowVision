package toolgood.flowVision.Flows;

import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;

import java.util.Map;

public class FactoryProcedureWork {
    private ProjectWork Project;// { get; set; }
    public String Category;
    public String Code;
    public String Name;
    public String CheckFormula;
    public Map<String, FactoryProcedureItemWork> Items;

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
