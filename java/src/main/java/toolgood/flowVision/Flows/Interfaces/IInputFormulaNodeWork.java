package toolgood.flowVision.Flows.Interfaces;

import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.ProjectWork;
import toolgood.flowVision.Flows.SettingFormulaItemWork;

import java.util.List;

public interface IInputFormulaNodeWork {
    ProjectWork Project();

    String InputName();

    List<SettingFormulaItemWork> InputFormula();

    boolean IsSubsidiaryCount();

    double EvaluateInputNum(FlowEngine engine) throws Exception;

    String GetMatchFormula(FlowEngine engine) throws Exception;

}
