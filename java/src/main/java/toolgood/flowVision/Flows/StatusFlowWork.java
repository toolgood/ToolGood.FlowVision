package toolgood.flowVision.Flows;

import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Enums.InputType;
import toolgood.flowVision.Flows.Interfaces.ISettingFormulaNodeWork;

import java.util.List;

public class StatusFlowWork extends NodeWork implements ISettingFormulaNodeWork {

    public List<SettingFormulaWork> SettingFormula;
    public String CheckFormula;
    public String Status;
    public String StatusCheckFormula;

    public StatusFlowWork() {
        NodeType = CellType.Status;
    }

    @Override
    public void Init(ProjectWork work, AppWork app) {
        super.Init(work, app);
        for (int i = 0; i < SettingFormula.size(); i++) {
            SettingFormulaWork item = SettingFormula.get(i);
            item.Init(work);
            item.NodeWork = this;
        }
    }

    @Override
    public boolean Check(FlowEngine engine, String factoryCode) throws Exception {
        if (CheckFormula == null || CheckFormula.equals("")) {
            return true;
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(CheckFormula);
        return engine.TryEvaluate(progContext, false);
    }

    public boolean CheckStatus(FlowEngine engine) throws Exception {
        if (CheckFormula == null || CheckFormula.equals("")) {
            return true;
        }
        var progContext = Project.CreateProgContext(StatusCheckFormula);
        Operand operand = engine.EvaluateFormula(progContext, InputType.Bool);
        return operand.BooleanValue();
    }


    @Override
    public List<SettingFormulaWork> SettingFormula() {
        return SettingFormula;
    }
}
