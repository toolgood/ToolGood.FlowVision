package toolgood.flowVision.Flows;

import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.InputType;

import java.util.List;

public class SettingFormulaWork {
    public ProjectWork Project;// { get; set; }

    public NodeWork NodeWork ;

    public String Name ;
    public InputType Type ;
    public String DefaultFormula ;
    public List<SettingFormulaItemWork> Conditions;
    public String Comment ;

    public Operand EvaluateFormula(FlowEngine engine) throws Exception {
        if (Conditions != null && Conditions.size() > 0) {
            for (int i = 0; i < Conditions.size(); i++) {
                SettingFormulaItemWork settingFormulaItem = Conditions.get(i);
                if (settingFormulaItem.Check(engine)) {
                    return settingFormulaItem.EvaluateFormula(engine, Type);
                }
            }
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(DefaultFormula);
        return engine.EvaluateFormula(progContext, (int)Type);
    }
    public void Init(ProjectWork work)
    {
        Project = work;
        if (Conditions != null && Conditions.size() > 0) {
            for (int i = 0; i < Conditions.size(); i++) {
                SettingFormulaItemWork settingFormulaItem = Conditions.get(i);
                settingFormulaItem.Init(work);
            }
        }
    }

    public String GetMatchFormula(FlowEngine engine) throws Exception {
        if (Conditions != null && Conditions.size() > 0) {
            for (int i = 0; i < Conditions.size(); i++) {
                SettingFormulaItemWork settingFormulaItem = Conditions.get(i);
                if (settingFormulaItem.Check(engine)) {
                    return settingFormulaItem.Formula;
                }
            }
        }
        return DefaultFormula;
    }
}
