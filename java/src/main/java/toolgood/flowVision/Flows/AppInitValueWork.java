package toolgood.flowVision.Flows;

import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.InputType;

import java.util.List;
import java.util.concurrent.locks.Condition;

public class AppInitValueWork {
    private ProjectWork Project;// { get; set; }
    public String Name ;
    public toolgood.flowVision.Flows.Enums.InputType InputType;
    public List<SettingFormulaItemWork> Conditions;

    public boolean IsThrowError;
    public String ErrorMessage;

    public void Init(ProjectWork work)
    {
        Project = work;
        for (int i = 0; i < Conditions.size(); i++) {
            SettingFormulaItemWork item = Conditions.get(i);
            item.Init(work);
        }
    }

    public String GetMatchFormula(FlowEngine engine) throws Exception {
        for (int i = 0; i < Conditions.size(); i++) {
            SettingFormulaItemWork item = Conditions.get(i);
            if (item.Check(engine)) {
                return item.Formula;
            }
        }
        return null;
    }
}
