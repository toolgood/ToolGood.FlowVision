using System.Text.Json;
using System.Text.Json.Serialization;
using ToolGood.FlowVision.Engines;

namespace ToolGood.FlowVision.Flows
{
    [JsonConverter(typeof(NodeWorkConverter))]
    public abstract class NodeWork
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public ProjectWork Project { get; set; }

        public abstract CellType NodeType { get; }
        public string Id { get; set; }
        public string Label { get; set; }
        public int Layer { get; set; }
        public string Comment { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Dictionary<string, List<NodeWork>> NextNodes;// { get; set; }
        public Dictionary<string, List<string>> NextNodeIds { get; set; }

        internal virtual bool Check(FlowEngine engine, string factoryCode) => true;
        internal List<string> GetChannels() => NextNodes.Keys.ToList();
        internal virtual void Init(ProjectWork work, AppWork app)
        {
            Project = work;
            if (NextNodeIds == null) { return; }
            NextNodes = new Dictionary<string, List<NodeWork>>();
            foreach (var ids in NextNodeIds) {
                var list = new List<NodeWork>();
                for (int i = 0; i < ids.Value.Count; i++) {
                    var id = ids.Value[i];
                    if (app.AllNodeWork.TryGetValue(id, out NodeWork nodeWork)) {
                        list.Add(nodeWork);
                    }
                }
                if (list.Count > 0) {
                    NextNodes.Add(ids.Key, list);
                }
            }
        }
    }
    public class NodeWorkConverter : JsonConverter<NodeWork>
    {
        public override NodeWork Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader typeReader = reader;
            if (typeReader.TokenType != JsonTokenType.StartObject) { throw new JsonException(); }
            while (typeReader.Read()) {
                if (typeReader.TokenType != JsonTokenType.PropertyName) { throw new JsonException(); }
                string propertyName = typeReader.GetString();
                if (propertyName.Equals(nameof(NodeWork.NodeType), StringComparison.OrdinalIgnoreCase)) { break; }
                typeReader.Skip();
            }
            if (!typeReader.Read() || typeReader.TokenType != JsonTokenType.Number) { throw new JsonException(); }

            CellType type = (CellType)typeReader.GetInt32();
            switch (type) {
                case CellType.Start: return JsonSerializer.Deserialize<StartFlowWork>(ref reader, options);
                case CellType.End: return JsonSerializer.Deserialize<EndFlowWork>(ref reader,  options);
                case CellType.Error: return JsonSerializer.Deserialize<ErrorFlowWork>(ref reader,  options);
                case CellType.Procedure: return JsonSerializer.Deserialize<ProcedureFlowWork>(ref reader, options);
                case CellType.Custom: return JsonSerializer.Deserialize<CustomFlowWork>(ref reader,  options);
                case CellType.Jump: return JsonSerializer.Deserialize<JumpFlowWork>(ref reader,  options);
                case CellType.Merge: return JsonSerializer.Deserialize<MergeFlowWork>(ref reader,  options);
                case CellType.Status: return JsonSerializer.Deserialize<StatusFlowWork>(ref reader,  options);
                default: break;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, NodeWork value, JsonSerializerOptions options)
        {
            if (value is null) { writer.WriteNullValue(); return; }
            switch (value) {
                case StartFlowWork flowWork: JsonSerializer.Serialize(writer, flowWork, options); break;
                case EndFlowWork flowWork: JsonSerializer.Serialize(writer, flowWork, options); break;
                case ErrorFlowWork flowWork: JsonSerializer.Serialize(writer, flowWork, options); break;
                case ProcedureFlowWork flowWork: JsonSerializer.Serialize(writer, flowWork, options); break;
                case CustomFlowWork flowWork: JsonSerializer.Serialize(writer, flowWork, options); break;
                case JumpFlowWork flowWork: JsonSerializer.Serialize(writer, flowWork, options); break;
                case MergeFlowWork flowWork: JsonSerializer.Serialize(writer, flowWork, options); break;
                case StatusFlowWork flowWork: JsonSerializer.Serialize(writer, flowWork, options); break;
                default: throw new NotSupportedException();
            }
        }
    }

    public sealed class StartFlowWork : NodeWork, ISettingFormulaNodeWork
    {
        public override CellType NodeType => CellType.Start;
        public List<SettingFormulaWork> SettingFormula { get; set; }
        internal override void Init(ProjectWork work, AppWork app)
        {
            base.Init(work, app);
            for (int i = 0; i < SettingFormula.Count; i++) {
                var item = SettingFormula[i];
                item.Init(work);
                item.NodeWork = this;
            }
        }
    }

    public sealed class EndFlowWork : NodeWork, ISettingFormulaNodeWork
    {
        public override CellType NodeType => CellType.End;
        public List<SettingFormulaWork> SettingFormula { get; set; }
        internal override void Init(ProjectWork work, AppWork app)
        {
            base.Init(work, app);
            for (int i = 0; i < SettingFormula.Count; i++) {
                var item = SettingFormula[i];
                item.Init(work);
                item.NodeWork = this;
            }
        }
    }
    public sealed class ErrorFlowWork : NodeWork
    {
        public override CellType NodeType => CellType.Error;
        public string CheckFormula { get; set; }
        public string ErrorMessage { get; set; }

        internal override bool Check(FlowEngine engine, string factoryCode)
        {
            if (string.IsNullOrEmpty(CheckFormula)) { return true; }
            var progContext = Project.CreateProgContext(CheckFormula);
            return engine.TryEvaluate(progContext, false);
        }
    }
    public sealed class ProcedureFlowWork : NodeWork, ISettingFormulaNodeWork, IInputNameNodeWork, IInputFormulaNodeWork
    {
        public override CellType NodeType => CellType.Procedure;
        public string Procedure { get; set; }
        public string CheckFormula { get; set; }
        public CheckType CheckType { get; set; }
        public string InputName { get; set; }
        public InputNumberType NumberType { get; set; }
        public bool IsSubsidiaryCount { get; set; }
        public bool MachineRequired { get; set; }
        public List<SettingFormulaWork> SettingFormula { get; set; }
        public List<SettingFormulaItemWork> InputFormula { get; set; }
        public Dictionary<string, List<ProcedureFlowMachineInfo>> Machines { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public FactoryProcedureWork FactoryProcedure { get; set; }


        internal override bool Check(FlowEngine engine, string factoryCode)
        {
            if (string.IsNullOrEmpty(CheckFormula) == false) {
                var progContext = Project.CreateProgContext(CheckFormula);
                var operand = engine.EvaluateFormula(progContext, (int)Flows.InputType.Bool);
                if (operand.BooleanValue == false) { return false; }
            }
            if (CheckType == CheckType.Add) {
                if (FactoryProcedure.Check(engine) == false) {
                    return false;
                }
            }
            if (InputFormula != null && InputFormula.Count > 0) {
                bool check = false;
                for (int i = 0; i < InputFormula.Count; i++) {
                    var inputFormula = InputFormula[i];
                    if (inputFormula.Check(engine)) { check = true; }
                }
                if (check == false) { return false; }
            }
            if (FactoryProcedure.Items.ContainsKey(factoryCode) == false) { return false; }

            if (Machines.TryGetValue(factoryCode, out List<ProcedureFlowMachineInfo> list)) {
                for (int i = 0; i < list.Count; i++) {
                    var item = list[i];
                    if (item.Check(engine)) { return true; }
                }
            }
            if (MachineRequired) { return false; }
            return true;
        }

        internal bool CheckWithoutItems(FlowEngine engine, string factoryCode)
        {
            if (string.IsNullOrEmpty(CheckFormula) == false) {
                var progContext = Project.CreateProgContext(CheckFormula);
                var operand = engine.EvaluateFormula(progContext, (int)Flows.InputType.Bool);
                if (operand.BooleanValue == false) { return false; }
            }
            if (CheckType == CheckType.Add) {
                if (FactoryProcedure.Check(engine) == false) {
                    return false;
                }
            }
            if (InputFormula == null || InputFormula.Count == 0) {
                return true;
            }
            bool check = false;
            for (int i = 0; i < InputFormula.Count; i++) {
                var inputFormula = InputFormula[i];
                if (inputFormula.Check(engine)) { check = true; }
            }
            if (check == false) { return false; }
            return FactoryProcedure.Items.ContainsKey(factoryCode);
        }
        internal bool CheckWithoutFactory(FlowEngine engine)
        {
            if (string.IsNullOrEmpty(CheckFormula) == false) {
                var progContext = Project.CreateProgContext(CheckFormula);
                var operand = engine.EvaluateFormula(progContext, (int)Flows.InputType.Bool);
                if (operand.BooleanValue == false) { return false; }
            }
            if (CheckType == CheckType.Add) {
                if (FactoryProcedure.Check(engine) == false) {
                    return false;
                }
            }
            if (InputFormula == null || InputFormula.Count == 0) {
                return true;
            }
            bool check = false;
            for (int i = 0; i < InputFormula.Count; i++) {
                var inputFormula = InputFormula[i];
                if (inputFormula.Check(engine)) { check = true; }
            }
            return check;
        }

        internal override void Init(ProjectWork work, AppWork app)
        {
            base.Init(work, app);
            FactoryProcedure = Project.FactoryProcedureList[Procedure];
            for (int i = 0; i < SettingFormula.Count; i++) {
                var item = SettingFormula[i];
                item.Init(work);
                item.NodeWork = this;
            }
            if (InputFormula != null && InputFormula.Count > 0) {
                for (int i = 0; i < InputFormula.Count; i++) {
                    var inputFormula = InputFormula[i];
                    inputFormula.Init(work);
                }
            }
            foreach (var machine in Machines) {
                for (int i = 0; i < machine.Value.Count; i++) {
                    var item = machine.Value[i];
                    item.Init(work);
                }
            }
        }

    }
    public sealed class ProcedureFlowMachineInfo
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public ProjectWork Project { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public CheckType CheckType { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public FactoryMachineWork FactoryMachine { get; set; }

        internal void Init(ProjectWork work)
        {
            Project = work;
            FactoryMachine = work.FactoryMachineList[Name];
        }
        internal bool Check(FlowEngine engine)
        {
            if (string.IsNullOrEmpty(Condition) == false) {
                var progContext = Project.CreateProgContext(Condition);
                var operand = engine.EvaluateFormula(progContext, (int)Flows.InputType.Bool);
                if (operand.BooleanValue == false) { return false; }
            }
            if (CheckType == CheckType.Add) {
                if (FactoryMachine.Check(engine) == false) {
                    return false;
                }
            }
            return true;
        }

    }


    public sealed class CustomFlowWork : NodeWork
    {
        public override CellType NodeType => CellType.Custom;
        public string CheckFormula { get; set; }
        public string Script { get; set; }
        public List<string> Names { get; set; }

        internal override bool Check(FlowEngine engine, string factoryCode)
        {
            if (string.IsNullOrEmpty(CheckFormula)) { return true; }
            var progContext = Project.CreateProgContext(CheckFormula);
            return engine.TryEvaluate(progContext, false);
        }

    }

    public sealed class JumpFlowWork : NodeWork, ISettingFormulaNodeWork, IInputNameNodeWork
    {
        public override CellType NodeType => CellType.Jump;
        public string InputName { get; set; }
        public string CheckFormula { get; set; }

        public List<SettingFormulaWork> SettingFormula { get; set; }

        internal override void Init(ProjectWork work, AppWork app)
        {
            base.Init(work, app);
            for (int i = 0; i < SettingFormula.Count; i++) {
                var item = SettingFormula[i];
                item.Init(work);
                item.NodeWork = this;
            }
        }
        internal override bool Check(FlowEngine engine, string factoryCode)
        {
            if (string.IsNullOrEmpty(CheckFormula)) { return true; }
            var progContext = Project.CreateProgContext(CheckFormula);
            return engine.TryEvaluate(progContext, false);
        }
    }

    public sealed class MergeFlowWork : NodeWork, ISettingFormulaNodeWork
    {
        public override CellType NodeType => CellType.Merge;
        public List<SettingFormulaWork> SettingFormula { get; set; }
        internal override void Init(ProjectWork work, AppWork app)
        {
            base.Init(work, app);
            for (int i = 0; i < SettingFormula.Count; i++) {
                var item = SettingFormula[i];
                item.Init(work);
                item.NodeWork = this;
            }
        }
    }

    public sealed class StatusFlowWork : NodeWork, ISettingFormulaNodeWork
    {
        public override CellType NodeType => CellType.Status;
        public List<SettingFormulaWork> SettingFormula { get; set; }
        public string CheckFormula { get; set; }
        public string Status { get; set; }
        public string StatusCheckFormula { get; set; }

        internal override void Init(ProjectWork work, AppWork app)
        {
            base.Init(work, app);
            for (int i = 0; i < SettingFormula.Count; i++) {
                var item = SettingFormula[i];
                item.Init(work);
                item.NodeWork = this;
            }
        }

        internal override bool Check(FlowEngine engine, string factoryCode)
        {
            if (string.IsNullOrEmpty(CheckFormula)) { return true; }
            var progContext = Project.CreateProgContext(CheckFormula);
            return engine.TryEvaluate(progContext, false);
        }

        internal bool CheckStatus(FlowEngine engine)
        {
            if (string.IsNullOrEmpty(StatusCheckFormula)) { return true; }
            var progContext = Project.CreateProgContext(StatusCheckFormula);
            var operand = engine.EvaluateFormula(progContext, (int)Flows.InputType.Bool);
            return operand.BooleanValue;
        }



    }

}
