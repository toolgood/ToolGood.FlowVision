package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;
import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.InputType;

public class ProcedureFlowMachineInfo {
    public ProjectWork Project;
    public String Name;
    public String Condition;
    public toolgood.flowVision.Flows.Enums.CheckType CheckType;
    public FactoryMachineWork FactoryMachine;

    final static ProcedureFlowMachineInfo parse(JSONObject jsonObject) {
        ProcedureFlowMachineInfo result = new ProcedureFlowMachineInfo();
        result.Name = jsonObject.getString("name");
        result.Condition = jsonObject.getString("condition");
        result.CheckType = toolgood.flowVision.Flows.Enums.CheckType.intToEnum(jsonObject.getIntValue("checkType"));

        return result;
    }

    public void Init(ProjectWork work) {
        Project = work;
        FactoryMachine = work.FactoryMachineList.get(Name);
    }

    public boolean Check(FlowEngine engine) throws Exception {
        if (Condition != null && Condition.equals("") == false) {
            mathParser.ProgContext progContext = Project.CreateProgContext(Condition);
            Operand operand = engine.EvaluateFormula(progContext, InputType.Bool);
            if (operand.BooleanValue() == false) {
                return false;
            }
        }
        if (CheckType == toolgood.flowVision.Flows.Enums.CheckType.Add) {
            return FactoryMachine.Check(engine) != false;
        }
        return true;
    }
}
