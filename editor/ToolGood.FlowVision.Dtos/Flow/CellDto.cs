using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using ToolGood.FlowVision.Dtos.Projects.Dtos;

namespace ToolGood.FlowVision.Dtos.Flow
{
	public abstract class CellDto
	{
		public abstract CellType NodeType { get; }
		public string Id { get; internal set; }
		public string Label { get; internal set; }
		public string Comment { get; internal set; }

		public bool IsUsed { get; internal set; }

		public static CellDto Parse(JToken jObject)
		{
			var shape = jObject["shape"].ToSafeString();

			if ("dag-edge" == shape) {
				return EdgeDto.ParseItem(jObject);
			} else if ("start" == shape) {
				return StartFlowDto.ParseItem(jObject);
			} else if ("end" == shape) {
				return EndFlowDto.ParseItem(jObject);
			} else if ("error" == shape) {
				return ErrorFlowDto.ParseItem(jObject);
			} else if ("procedure" == shape) {
				return ProcedureFlowDto.ParseItem(jObject);
			} else if ("jump" == shape) {
				return JumpFlowDto.ParseItem(jObject);
			} else if ("merge" == shape) {
				return MergeFlowDto.ParseItem(jObject);
			} else if ("custom" == shape) {
				return CustomFlowDto.ParseItem(jObject);
			} else if ("comment" == shape) {
				return null;
			} else if ("status" == shape) {
				return StatusFlowDto.ParseItem(jObject);
			}
			throw new InvalidDataException("shape is error.");
		}
	}

	public class EdgeDto : CellDto
	{
		public override CellType NodeType => CellType.Edge;
		public string SourceCell { get; internal set; }
		public string TargetCell { get; internal set; }

		internal static EdgeDto ParseItem(JToken jObject)
		{
			EdgeDto edge = new EdgeDto();
			edge.Id = jObject["id"].ToSafeString();
			edge.Label = jObject["labels"][0]["attrs"]["label"]["text"].ToSafeString();
			if (jObject["data"] != null) {
				edge.Comment = jObject["data"]["comment"].ToSafeString();
			}
			edge.SourceCell = jObject["source"]["cell"].ToSafeString();
			edge.TargetCell = jObject["target"]["cell"].ToSafeString();
			return edge;
		}
	}

	public abstract class NodeDto : CellDto
	{
		public int Layer { get; internal set; }
		public int Order { get; internal set; }
		public Dictionary<string, List<NodeDto>> NextNodes { get; internal set; } = new Dictionary<string, List<NodeDto>>();
	}

	public class SettingFormulaDto
	{
		public string Name { get; internal set; }
		public InputType Type { get; internal set; }
		public string DefaultFormula { get; internal set; }
		public List<SettingFormulaItem> Conditions { get; internal set; } = new List<SettingFormulaItem>();
		public string Comment { get; internal set; }

		internal static SettingFormulaDto ParseItem(JToken jObject)
		{
			var json = JObject.Parse(jObject.Value<string>());

			SettingFormulaDto item = new SettingFormulaDto();
			item.Name = json["name"].ToSafeString();
			var type = json["type"].ToSafeString();
			if (type == "number") {
				item.Type = InputType.Number;
			} else if (type == "string") {
				item.Type = InputType.String;
			} else if (type == "date") {
				item.Type = InputType.Date;
			} else if (type == "bool") {
				item.Type = InputType.Bool;
			} else if (type == "list") {
				item.Type = InputType.List;
			}
			item.DefaultFormula = json["defaultFormula"].ToSafeString();
			item.Comment = json["comment"].ToSafeString();
			item.Conditions = new List<SettingFormulaItem>();
			if (json["conditions"] != null) {
				var arrs = json["conditions"] as JArray;
				foreach (var a in arrs) {
					var i = SettingFormulaItem.ParseItem(a);
					item.Conditions.Add(i);
				}
			}
			return item;
		}
	}

	public class SettingFormulaItem
	{
		public string Condition { get; set; }
		public string Formula { get; set; }

		internal static SettingFormulaItem ParseItem(JToken jObject)
		{
			SettingFormulaItem item = new SettingFormulaItem();
			item.Condition = jObject["condition"].ToSafeString();
			item.Formula = jObject["formula"].ToSafeString();
			return item;
		}
	}

	public class StartFlowDto : NodeDto
	{
		public override CellType NodeType => CellType.Start;
		public List<SettingFormulaDto> SettingFormula { get; internal set; }

		internal static StartFlowDto ParseItem(JToken jObject)
		{
			var flowDto = new StartFlowDto();
			flowDto.Id = jObject["id"].ToSafeString();
			flowDto.Comment = jObject["data"]["comment"].ToSafeString();
			flowDto.Label = jObject["attrs"]["text"]["text"].ToSafeString();
			flowDto.Layer = int.Parse(jObject["data"]["layer"].ToString());
			flowDto.Order = int.Parse(jObject["data"]["order"].ToString());

			flowDto.SettingFormula = new List<SettingFormulaDto>();
			if (jObject["data"]["settingFormula"] != null) {
				var arrs = jObject["data"]["settingFormula"] as JArray;
				foreach (var a in arrs) {
					flowDto.SettingFormula.Add(SettingFormulaDto.ParseItem(a));
				}
			}
			return flowDto;
		}
	}

	public class EndFlowDto : NodeDto
	{
		public override CellType NodeType => CellType.End;

		public List<SettingFormulaDto> SettingFormula { get; internal set; }

		internal static EndFlowDto ParseItem(JToken jObject)
		{
			var flowDto = new EndFlowDto();
			flowDto.Id = jObject["id"].ToSafeString();
			flowDto.Comment = jObject["data"]["comment"].ToSafeString();
			flowDto.Label = jObject["attrs"]["text"]["text"].ToSafeString();
			flowDto.Layer = int.Parse(jObject["data"]["layer"].ToString());
			flowDto.Order = int.Parse(jObject["data"]["order"].ToString());

			flowDto.SettingFormula = new List<SettingFormulaDto>();
			if (jObject["data"]["settingFormula"] != null) {
				var arrs = jObject["data"]["settingFormula"] as JArray;
				foreach (var a in arrs) {
					flowDto.SettingFormula.Add(SettingFormulaDto.ParseItem(a));
				}
			}

			return flowDto;
		}
	}

	public class ErrorFlowDto : NodeDto
	{
		public override CellType NodeType => CellType.Error;
		public string ErrorMessage { get; internal set; }
		public string CheckFormula { get; internal set; }

		internal static ErrorFlowDto ParseItem(JToken jObject)
		{
			var flowDto = new ErrorFlowDto();
			flowDto.Id = jObject["id"].ToSafeString();
			flowDto.Comment = jObject["data"]["comment"].ToSafeString();
			flowDto.Label = jObject["attrs"]["text"]["text"].ToSafeString();
			flowDto.Layer = int.Parse(jObject["data"]["layer"].ToString());
			flowDto.Order = int.Parse(jObject["data"]["order"].ToString());

			flowDto.ErrorMessage = jObject["data"]["errorMessage"].ToString();
			flowDto.CheckFormula = jObject["data"]["checkFormula"].ToString();
			return flowDto;
		}
	}

	public class ProcedureFlowDto : NodeDto
	{
		public override CellType NodeType => CellType.Procedure;
		public string Procedure { get; internal set; }
		public string CheckFormula { get; internal set; }
		public CheckType CheckType { get; internal set; }
		public List<SettingFormulaDto> SettingFormula { get; internal set; }
		public List<SettingFormulaItem> InputFormula { get; internal set; }
		public List<MachineInfo> Machines { get; internal set; }
		public bool MachineRequired { get; internal set; }

		public string InputName { get; internal set; }
		public InputNumberType NumberType { get; internal set; }
		public bool IsSubsidiaryCount { get; internal set; }
		public FactoryProcedureDto FactoryProcedure { get; internal set; }

		internal static ProcedureFlowDto ParseItem(JToken jObject)
		{
			var flowDto = new ProcedureFlowDto();
			flowDto.Id = jObject["id"].ToSafeString();
			flowDto.Comment = jObject["data"]["comment"].ToSafeString();
			flowDto.Label = jObject["attrs"]["text"]["text"].ToSafeString();
			flowDto.Layer = int.Parse(jObject["data"]["layer"].ToString());
			flowDto.Order = int.Parse(jObject["data"]["order"].ToString());

			flowDto.Procedure = jObject["data"]["procedure"].ToString();
			flowDto.CheckFormula = jObject["data"]["checkFormula"].ToString();
			var type = jObject["data"]["checkType"].ToString();
			flowDto.CheckType = type == "add" ? CheckType.Add : CheckType.Replace;

			flowDto.SettingFormula = new List<SettingFormulaDto>();
			if (jObject["data"]["settingFormula"] != null) {
				var arrs = jObject["data"]["settingFormula"] as JArray;
				foreach (var a in arrs) {
					flowDto.SettingFormula.Add(SettingFormulaDto.ParseItem(a));
				}
			}
			flowDto.InputFormula = new List<SettingFormulaItem>();
			if (jObject["data"]["inputFormula"] != null) {
				var arrs = jObject["data"]["inputFormula"] as JArray;
				foreach (var a in arrs) {
					flowDto.InputFormula.Add(SettingFormulaItem.ParseItem(a));
				}
			}
			flowDto.Machines = new List<MachineInfo>();
			if (jObject["data"]["machines"] != null) {
				var arrs = jObject["data"]["machines"] as JArray;
				foreach (var a in arrs) {
					flowDto.Machines.Add(MachineInfo.ParseItem(a));
				}
			}
			if (jObject["data"]["machineRequired"] != null) {
				flowDto.MachineRequired = bool.Parse(jObject["data"]["machineRequired"].ToString());
			}
			flowDto.NumberType = InputNumberType.Original;
			if (jObject["data"]["inputNumberType"] != null) {
				var inputNumberType = jObject["data"]["inputNumberType"].ToString();
				if (inputNumberType == "ceil") {
					flowDto.NumberType = InputNumberType.Ceil;
				} else if (inputNumberType == "floor") {
					flowDto.NumberType = InputNumberType.Floor;
				} else if (inputNumberType == "round0") {
					flowDto.NumberType = InputNumberType.Round0;
				} else if (inputNumberType == "round1") {
					flowDto.NumberType = InputNumberType.Round1;
				} else if (inputNumberType == "round2") {
					flowDto.NumberType = InputNumberType.Round2;
				} else if (inputNumberType == "round3") {
					flowDto.NumberType = InputNumberType.Round3;
				} else if (inputNumberType == "round4") {
					flowDto.NumberType = InputNumberType.Round4;
				} else if (inputNumberType == "round5") {
					flowDto.NumberType = InputNumberType.Round5;
				} else if (inputNumberType == "round6") {
					flowDto.NumberType = InputNumberType.Round6;
				} else if (inputNumberType == "round7") {
					flowDto.NumberType = InputNumberType.Round7;
				} else if (inputNumberType == "round8") {
					flowDto.NumberType = InputNumberType.Round8;
				}
			}

			flowDto.InputName = jObject["data"]["inputName"].ToString();
			flowDto.IsSubsidiaryCount = jObject["data"]["isSubsidiaryCount"].ToSafeString() == "true";
			return flowDto;
		}

		public class MachineInfo
		{
			public string Condition { get; set; }
			public string Name { get; set; }
			public CheckType CheckType { get; set; }
			public string FactoryCode { get; set; }
			public bool IsUsed { get; set; }
			public FactoryMachineDto FactoryMachine { get; set; }

			internal static MachineInfo ParseItem(JToken jObject)
			{
				var flowDto = new MachineInfo();
				flowDto.Condition = jObject["condition"].ToSafeString();
				flowDto.Name = jObject["name"].ToSafeString();
				flowDto.FactoryCode = jObject["factoryCode"].ToSafeString();
				var type = jObject["checkType"].ToString();
				flowDto.CheckType = type == "add" ? CheckType.Add : CheckType.Replace;
				return flowDto;
			}
		}
	}

	public class CustomFlowDto : NodeDto
	{
		private static Regex nameRegex = new Regex(@"\bsetValue\([\""']([^\""']+)[\""']", RegexOptions.Compiled);
		private static Regex getnameRegex = new Regex(@"\bgetValue\([\""']([^\""']+)[\""']\)", RegexOptions.Compiled);
		private static Regex multiCommentRegex = new Regex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled);
		private static Regex commentRegex = new Regex(@"//.*", RegexOptions.Compiled);

		public override CellType NodeType => CellType.Custom;
		public string CheckFormula { get; set; }
		public string Script { get; set; }
		public List<string> Names { get; set; }
		public List<string> GetNames { get; set; }

		internal static CustomFlowDto ParseItem(JToken jObject)
		{
			var flowDto = new CustomFlowDto();
			flowDto.Id = jObject["id"].ToSafeString();
			flowDto.Comment = jObject["data"]["comment"].ToSafeString();
			flowDto.Label = jObject["attrs"]["text"]["text"].ToSafeString();
			flowDto.Layer = int.Parse(jObject["data"]["layer"].ToString());
			flowDto.Order = int.Parse(jObject["data"]["order"].ToString());

			flowDto.CheckFormula = jObject["data"]["checkFormula"].ToString();
			flowDto.Script = jObject["data"]["script"].ToString();
			flowDto.Names = new List<string>();
			flowDto.GetNames = new List<string>();

			if (string.IsNullOrEmpty(flowDto.Script) == false) {
				var script = flowDto.Script;
				script = multiCommentRegex.Replace(script, "");//去除注释 防止出错
				script = commentRegex.Replace(script, "");//去除注释 防止出错

				var ms = nameRegex.Matches(script);
				foreach (Match item in ms) {
					flowDto.Names.Add(item.Groups[1].Value);
				}
				var ms2 = getnameRegex.Matches(script);
				foreach (Match item in ms2) {
					flowDto.GetNames.Add(item.Groups[1].Value);
				}
			}
			return flowDto;
		}
	}

	public class JumpFlowDto : NodeDto
	{
		public override CellType NodeType => CellType.Jump;
		public List<SettingFormulaDto> SettingFormula { get; internal set; }
		public string InputName { get; internal set; }
		public string CheckFormula { get; internal set; }

		internal static JumpFlowDto ParseItem(JToken jObject)
		{
			var flowDto = new JumpFlowDto();
			flowDto.Id = jObject["id"].ToSafeString();
			flowDto.Comment = jObject["data"]["comment"].ToSafeString();
			flowDto.Label = jObject["attrs"]["text"]["text"].ToSafeString();
			flowDto.Layer = int.Parse(jObject["data"]["layer"].ToString());
			flowDto.Order = int.Parse(jObject["data"]["order"].ToString());

			flowDto.SettingFormula = new List<SettingFormulaDto>();
			if (jObject["data"]["settingFormula"] != null) {
				var arrs = jObject["data"]["settingFormula"] as JArray;
				foreach (var a in arrs) {
					flowDto.SettingFormula.Add(SettingFormulaDto.ParseItem(a));
				}
			}

			flowDto.InputName = jObject["data"]["inputName"].ToString();
			if (jObject["data"]["checkFormula"] != null) {
				flowDto.CheckFormula = jObject["data"]["checkFormula"].ToString();
			}
			return flowDto;
		}
	}

	public class MergeFlowDto : NodeDto
	{
		public override CellType NodeType => CellType.Merge;
		public List<SettingFormulaDto> SettingFormula { get; internal set; }

		internal static MergeFlowDto ParseItem(JToken jObject)
		{
			var flowDto = new MergeFlowDto();
			flowDto.Id = jObject["id"].ToSafeString();
			flowDto.Comment = jObject["data"]["comment"].ToSafeString();
			flowDto.Label = jObject["attrs"]["text"]["text"].ToSafeString();
			flowDto.Layer = int.Parse(jObject["data"]["layer"].ToString());
			flowDto.Order = int.Parse(jObject["data"]["order"].ToString());

			flowDto.SettingFormula = new List<SettingFormulaDto>();
			if (jObject["data"]["settingFormula"] != null) {
				var arrs = jObject["data"]["settingFormula"] as JArray;
				foreach (var a in arrs) {
					flowDto.SettingFormula.Add(SettingFormulaDto.ParseItem(a));
				}
			}

			return flowDto;
		}
	}

	public class StatusFlowDto : NodeDto
	{
		public override CellType NodeType => CellType.Status;
		public List<SettingFormulaDto> SettingFormula { get; set; }
		public string CheckFormula { get; set; }
		public string Status { get; set; }
		public string StatusCheckFormula { get; set; }

		internal static StatusFlowDto ParseItem(JToken jObject)
		{
			var flowDto = new StatusFlowDto();
			flowDto.Id = jObject["id"].ToSafeString();
			flowDto.Comment = jObject["data"]["comment"].ToSafeString();
			flowDto.Label = jObject["attrs"]["text"]["text"].ToSafeString();
			flowDto.Layer = int.Parse(jObject["data"]["layer"].ToString());
			flowDto.Order = int.Parse(jObject["data"]["order"].ToString());

			flowDto.CheckFormula = jObject["data"]["checkFormula"].ToString();
			flowDto.Status = jObject["data"]["status"].ToString();
			flowDto.StatusCheckFormula = jObject["data"]["statusCheckFormula"].ToString();

			flowDto.SettingFormula = new List<SettingFormulaDto>();
			if (jObject["data"]["settingFormula"] != null) {
				var arrs = jObject["data"]["settingFormula"] as JArray;
				foreach (var a in arrs) {
					flowDto.SettingFormula.Add(SettingFormulaDto.ParseItem(a));
				}
			}

			return flowDto;
		}
	}
}