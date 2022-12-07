using System.Text.Json.Serialization;
using ToolGood.Algorithm;
using ToolGood.FlowVision.Engines;

namespace ToolGood.FlowVision.Flows
{
    public sealed class SettingFormulaItemWork
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public ProjectWork Project;// { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Condition { get; set; }
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Formula { get; set; }


        internal Operand EvaluateFormula(FlowEngine engine, InputType inputType)
        {
            var progContext = Project.CreateProgContext(Formula);
            return engine.EvaluateFormula(progContext, (int)inputType);
        }

        internal void Init(ProjectWork work)
        {
            Project = work;
        }


        internal bool Check(FlowEngine engine)
        {
            if (string.IsNullOrEmpty(Condition)) { return true; }
            if (Condition == "1" || Condition.Equals("true", StringComparison.OrdinalIgnoreCase)) { return true; }
            var progContext = Project.CreateProgContext(Condition);
            return engine.TryEvaluate(progContext, false);
        }



    }
}
