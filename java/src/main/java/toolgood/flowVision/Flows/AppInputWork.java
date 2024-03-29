package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;
import toolgood.algorithm2.Operand;
import toolgood.algorithm2.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;

public class AppInputWork {
    public String InputName;
    public String Unit;
    public toolgood.flowVision.Flows.Enums.InputType InputType;
    public String CheckInput;
    public boolean IsRequired;
    public boolean UseDefaultValue;
    public String DefaultValue;
    public String ErrorMessage;
    private ProjectWork Project;

    final static AppInputWork parse(JSONObject jsonObject) {
        AppInputWork result = new AppInputWork();
        result.InputName = jsonObject.getString("inputName");
        result.Unit = jsonObject.getString("unit");
        result.InputType = toolgood.flowVision.Flows.Enums.InputType.intToEnum(jsonObject.getIntValue("inputType"));
        result.CheckInput = jsonObject.getString("checkInput");
        result.IsRequired = jsonObject.getBooleanValue("isRequired");
        result.UseDefaultValue = jsonObject.getBooleanValue("useDefaultValue");
        result.DefaultValue = jsonObject.getString("defaultValue");
        result.ErrorMessage = jsonObject.getString("errorMessage");

        return result;
    }

    public boolean Check(FlowEngine engine) throws Exception {
        if (CheckInput == null || CheckInput.equals("")) {
            return true;
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(CheckInput);
        Operand operand = engine.EvaluateFormula(progContext, toolgood.flowVision.Flows.Enums.InputType.Bool);
        return operand.BooleanValue();
    }

    public void Init(ProjectWork work) {
        Project = work;
    }
}
