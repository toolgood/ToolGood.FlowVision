package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;
import toolgood.algorithm2.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;

public class FactoryMachineWork {
    public String Factory;
    public String MachineCode;
    public String MachineName;
    public String MachineCategoryCode;
    public String MachineCategoryName;
    public String CheckFormula;
    private ProjectWork Project;

    final static FactoryMachineWork parse(JSONObject jsonObject) {
        FactoryMachineWork result = new FactoryMachineWork();
        result.Factory = jsonObject.getString("factory");
        result.MachineCode = jsonObject.getString("machineCode");
        result.MachineName = jsonObject.getString("machineName");
        result.MachineCategoryCode = jsonObject.getString("machineCategoryCode");
        result.MachineCategoryName = jsonObject.getString("machineCategoryName");
        result.CheckFormula = jsonObject.getString("checkFormula");

        return result;
    }

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
