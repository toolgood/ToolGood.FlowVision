package toolgood.flowVision.Flows;

import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Interfaces.IInputNameNodeWork;
import toolgood.flowVision.Flows.Interfaces.ISettingFormulaNodeWork;

import java.util.List;

public class JumpFlowWork extends NodeWork implements ISettingFormulaNodeWork, IInputNameNodeWork {
    public String InputName;
    public String CheckFormula;

    public List<SettingFormulaWork> SettingFormula;

    public JumpFlowWork() {
        NodeType = CellType.Jump;
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

    @Override
    public String InputName() {
        return InputName;
    }

    @Override
    public List<SettingFormulaWork> SettingFormula() {
        return SettingFormula;
    }
}
