using System.Text.Json.Serialization;
using ToolGood.FlowVision.Engines;

namespace ToolGood.FlowVision.Flows
{
    public interface IInputFormulaNodeWork
    {
        [JsonIgnore]
        ProjectWork Project { get; set; }
        string InputName { get; set; }

        List<SettingFormulaItemWork> InputFormula { get; set; }

        bool IsSubsidiaryCount { get; set; }

        internal double EvaluateInputNum(FlowEngine engine)
        {
            if (InputFormula == null || InputFormula.Count == 0) {
                return engine.GetOutputNum().NumberValue;
            }
            for (int i = 0; i < InputFormula.Count; i++) {
                var settingFormulaItem = InputFormula[i];
                if (settingFormulaItem.Check(engine)) {
                    var operand = settingFormulaItem.EvaluateFormula(engine, InputType.Number);
                    return operand.NumberValue;
                }
            }
            throw new Exception("入量公式错误！");
        }

        internal string GetMatchFormula(FlowEngine engine)
        {
            if (InputFormula != null && InputFormula.Count > 0) {
                for (int i = 0; i < InputFormula.Count; i++) {
                    var settingFormulaItem = InputFormula[i];
                    if (settingFormulaItem.Check(engine)) {
                        return settingFormulaItem.Formula;
                    }
                }
            }
            return null;
        }


    }

}
