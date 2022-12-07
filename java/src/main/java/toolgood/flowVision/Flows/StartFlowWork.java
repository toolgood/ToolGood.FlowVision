package toolgood.flowVision.Flows;

import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Interfaces.ISettingFormulaNodeWork;

import java.util.List;

public class StartFlowWork extends NodeWork implements ISettingFormulaNodeWork {

    public List<SettingFormulaWork> SettingFormula;//{ get; set; }

    public StartFlowWork(){
        NodeType= CellType.Start;
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

}
