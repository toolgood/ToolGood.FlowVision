package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONArray;
import com.alibaba.fastjson2.JSONObject;
import toolgood.algorithm2.Operand;
import toolgood.algorithm2.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Enums.InputNumberType;
import toolgood.flowVision.Flows.Enums.InputType;
import toolgood.flowVision.Flows.Interfaces.IInputFormulaNodeWork;
import toolgood.flowVision.Flows.Interfaces.IInputNameNodeWork;
import toolgood.flowVision.Flows.Interfaces.ISettingFormulaNodeWork;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ProcedureFlowWork extends NodeWork implements ISettingFormulaNodeWork, IInputNameNodeWork, IInputFormulaNodeWork {
    public String Procedure;
    public String CheckFormula;
    public toolgood.flowVision.Flows.Enums.CheckType CheckType;
    public String InputName;
    public InputNumberType NumberType;
    public boolean IsSubsidiaryCount;
    public boolean MachineRequired;
    public List<SettingFormulaWork> SettingFormula;
    public List<SettingFormulaItemWork> InputFormula;
    public Map<String, List<ProcedureFlowMachineInfo>> Machines;
    public FactoryProcedureWork FactoryProcedure;

    public ProcedureFlowWork() {
        NodeType = CellType.Procedure;
    }

    final static ProcedureFlowWork parse2(JSONObject jsonObject) {
        ProcedureFlowWork result = new ProcedureFlowWork();
        result.Id = jsonObject.getString("id");
        result.Label = jsonObject.getString("label");
        result.Layer = jsonObject.getIntValue("layer");
        result.NodeType = CellType.intToEnum(jsonObject.getIntValue("nodeType"));
        result.NextNodeIds = new HashMap<>();
        if (jsonObject.containsKey("nextNodeIds")) {
            JSONObject nextNodeIds = jsonObject.getJSONObject("nextNodeIds");
            for (Map.Entry<String, Object> kv : nextNodeIds.entrySet()) {
                if (kv.getValue() instanceof JSONArray) {
                    List<String> list = new ArrayList<>();
                    JSONArray array = (JSONArray) kv.getValue();
                    for (Object obj : array) {
                        list.add(obj.toString());
                    }
                    result.NextNodeIds.put(kv.getKey(), list);
                }
            }
        }

        result.Procedure = jsonObject.getString("procedure");
        result.CheckFormula = jsonObject.getString("checkFormula");
        result.CheckType = toolgood.flowVision.Flows.Enums.CheckType.intToEnum(jsonObject.getIntValue("checkType"));
        result.InputName = jsonObject.getString("inputName");
        result.NumberType = InputNumberType.intToEnum(jsonObject.getIntValue("numberType"));
        result.IsSubsidiaryCount = jsonObject.getBooleanValue("isSubsidiaryCount");
        result.MachineRequired = jsonObject.getBooleanValue("machineRequired");

        result.SettingFormula = new ArrayList<>();
        JSONArray array = jsonObject.getJSONArray("settingFormula");
        for (Object s : array) {
            if (s instanceof JSONObject) {
                JSONObject jsonObject1 = (JSONObject) s;
                SettingFormulaWork work = SettingFormulaWork.parse(jsonObject1);
                if (work != null) {
                    result.SettingFormula.add(work);
                }
            }
        }
        result.InputFormula = new ArrayList<>();
        JSONArray array2 = jsonObject.getJSONArray("inputFormula");
        for (Object s : array2) {
            if (s instanceof JSONObject) {
                JSONObject jsonObject1 = (JSONObject) s;
                SettingFormulaItemWork work = SettingFormulaItemWork.parse(jsonObject1);
                if (work != null) {
                    result.InputFormula.add(work);
                }
            }
        }
        result.Machines = new HashMap<>();
        JSONArray array3 = jsonObject.getJSONArray("machines");
        for (Object s : array3) {
            if (s instanceof JSONObject) {
                JSONObject jsonObject1 = (JSONObject) s;
                for (Map.Entry<String, Object> item : jsonObject1.entrySet()) {
                    if (item.getValue() instanceof JSONArray) {
                        JSONArray jsonArray = (JSONArray) item.getValue();
                        List<ProcedureFlowMachineInfo> list = new ArrayList<>();
                        for (Object o : jsonArray) {
                            ProcedureFlowMachineInfo info = ProcedureFlowMachineInfo.parse((JSONObject) o);
                            list.add(info);
                        }
                        result.Machines.put(item.getKey(), list);
                    }
                }
            }
        }

        return result;
    }

    @Override
    public ProjectWork Project() {
        return Project;
    }

    @Override
    public List<SettingFormulaItemWork> InputFormula() {
        return InputFormula;
    }

    @Override
    public boolean IsSubsidiaryCount() {
        return IsSubsidiaryCount;
    }

    @Override
    public String InputName() {
        return InputName;
    }

    @Override
    public List<SettingFormulaWork> SettingFormula() {
        return SettingFormula;
    }

    @Override
    public boolean Check(FlowEngine engine, String factoryCode) throws Exception {
        if (CheckFormula != null && CheckFormula.equals("") == false) {
            mathParser.ProgContext progContext = Project.CreateProgContext(CheckFormula);
            Operand operand = engine.EvaluateFormula(progContext, InputType.Bool);
            if (operand.BooleanValue() == false) {
                return false;
            }
        }
        if (CheckType == toolgood.flowVision.Flows.Enums.CheckType.Add) {
            if (FactoryProcedure.Check(engine) == false) {
                return false;
            }
        }
        if (InputFormula != null && InputFormula.size() > 0) {
            boolean check = false;
            for (int i = 0; i < InputFormula.size(); i++) {
                SettingFormulaItemWork inputFormula = InputFormula.get(i);
                if (inputFormula.Check(engine)) {
                    check = true;
                }
            }
            if (check == false) {
                return false;
            }
        }
        if (FactoryProcedure.Items.containsKey(factoryCode) == false) {
            return false;
        }

        if (Machines.containsKey(factoryCode)) {
            List<ProcedureFlowMachineInfo> list = Machines.get(factoryCode);
            for (int i = 0; i < list.size(); i++) {
                ProcedureFlowMachineInfo item = list.get(i);
                if (item.Check(engine)) {
                    return true;
                }
            }
        }

        return !MachineRequired;
    }

    public boolean CheckWithoutFactory(FlowEngine engine) throws Exception {
        if (CheckFormula != null && CheckFormula.equals("") == false) {
            mathParser.ProgContext progContext = Project.CreateProgContext(CheckFormula);
            Operand operand = engine.EvaluateFormula(progContext, InputType.Bool);
            if (operand.BooleanValue() == false) {
                return false;
            }
        }
        if (CheckType == toolgood.flowVision.Flows.Enums.CheckType.Add) {
            if (FactoryProcedure.Check(engine) == false) {
                return false;
            }
        }
        if (InputFormula == null || InputFormula.size() == 0) {
            return true;
        }
        boolean check = false;
        for (int i = 0; i < InputFormula.size(); i++) {
            SettingFormulaItemWork inputFormula = InputFormula.get(i);
            if (inputFormula.Check(engine)) {
                check = true;
            }
        }
        return check;
    }

    @Override
    public void Init(ProjectWork work, AppWork app) {
        super.Init(work, app);
        FactoryProcedure = Project.FactoryProcedureList.get(Procedure);
        for (int i = 0; i < SettingFormula.size(); i++) {
            SettingFormulaWork item = SettingFormula.get(i);
            item.Init(work);
            item.NodeWork = this;
        }
        if (InputFormula != null && InputFormula.size() > 0) {
            for (int i = 0; i < InputFormula.size(); i++) {
                SettingFormulaItemWork inputFormula = InputFormula.get(i);
                inputFormula.Init(work);
            }
        }
        for (List<ProcedureFlowMachineInfo> machine : Machines.values()) {
            for (int i = 0; i < machine.size(); i++) {
                ProcedureFlowMachineInfo item = machine.get(i);
                item.Init(work);
            }
        }
    }

    @Override
    public BigDecimal EvaluateInputNum(FlowEngine engine) throws Exception {
        if (InputFormula == null || InputFormula.size() == 0) {
            return engine.GetOutputNum().NumberValue();
        }
        for (int i = 0; i < InputFormula.size(); i++) {
            SettingFormulaItemWork settingFormulaItem = InputFormula.get(i);
            if (settingFormulaItem.Check(engine)) {
                Operand operand = settingFormulaItem.EvaluateFormula(engine, InputType.Number);
                return operand.NumberValue();
            }
        }
        throw new Exception("入量公式错误！");
    }

    @Override
    public String GetMatchFormula(FlowEngine engine) throws Exception {
        if (InputFormula != null && InputFormula.size() > 0) {
            for (int i = 0; i < InputFormula.size(); i++) {
                SettingFormulaItemWork settingFormulaItem = InputFormula.get(i);
                if (settingFormulaItem.Check(engine)) {
                    return settingFormulaItem.Formula;
                }
            }
        }
        return null;
    }
}
