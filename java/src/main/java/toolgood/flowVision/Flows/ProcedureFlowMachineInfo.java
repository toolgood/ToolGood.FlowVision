package toolgood.flowVision.Flows;

import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CheckType;
import toolgood.flowVision.Flows.Enums.InputType;

public class ProcedureFlowMachineInfo extends  NodeWork{
    public ProjectWork Project ;
    public String Name ;
    public String Condition ;
    public toolgood.flowVision.Flows.Enums.CheckType CheckType ;
    public FactoryMachineWork FactoryMachine ;


    public void Init(ProjectWork work)
    {
        Project = work;
        FactoryMachine = work.FactoryMachineList.get(Name);
    }
    public boolean Check(FlowEngine engine) throws Exception {
        if (Condition!=null && Condition.equals("")==false) {
            mathParser.ProgContext  progContext = Project.CreateProgContext(Condition);
            Operand operand = engine.EvaluateFormula(progContext, InputType.Bool);
            if (operand.BooleanValue() == false) { return false; }
        }
        if (CheckType == CheckType.Add) {
            if (FactoryMachine.Check(engine) == false) {
                return false;
            }
        }
        return true;
    }


}
