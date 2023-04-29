using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ToolGood.FlowWork.Applications.Engines;
using ToolGood.FlowWork.Applications.Engines.OutDatas;
using ToolGood.FlowWork.Applications.Engines.Parameters;
using ToolGood.FlowWork.Commons;
using ToolGood.FlowWork.Commons.Controllers;
using ToolGood.FlowWork.Commons.My;
using ToolGood.FlowWork.Commons.Utils;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Pages
{
	[Microsoft.AspNetCore.Mvc.Route("/api/{action}")]
	public class ApiController : ApiControllerCore
	{
		#region 流程配置文件

		private ProjectWork GetProjectWork()
		{
			return ProjectWorkCache.GetProjectWork();
		}

		[HttpGet]
		[HttpPost]
		[IgnoreAntiforgeryToken]
		public IActionResult UpdateFile()
		{
			try {
				ProjectWorkCache.Update();
				GC.Collect();
				return Success();
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		[HttpPost]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> UploadFile(IFormFile file)
		{
			try {
				var filePath = MyHostingEnvironment.MapPath(Path.Combine("/App_Data", file.Name));
				using (var fileStream = new FileStream(filePath, FileMode.Create)) {
					await file.CopyToAsync(fileStream);
				}
				return Success();
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		#endregion 流程配置文件

		#region 计算公式、获取路径、获取公式步骤、获取公式条件、备选机械

		public IActionResult EvalStatus(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();
			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				if (work == null) { return Error("Please configure the project file information in appsettings.json ."); }
				FlowEngine engine = new FlowEngine(work, attachData, machines);
				var status = engine.BuildTreeNode(appCode, factoryCode, json, previous);

				engine.Dispose();
				engine = null;
				return Success(status);
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		[IgnoreAntiforgeryToken]
		public IActionResult EvalFormulas(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string formulas)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			if (string.IsNullOrWhiteSpace(formulas)) { return Error("formulas is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();
			formulas = formulas.Trim();
			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				FlowEngine engine = new FlowEngine(work, attachData, machines);
				var status = engine.BuildTreeNode(appCode, factoryCode, json, previous);
				Dictionary<string, string> result = new Dictionary<string, string>();
				if (status.Count > 0 && status[0].Equals("END")) { // 流程结束后，如果没有正确计算结果 直接返回错误
					engine.EvaluateInputNum();
					if (formulas.StartsWith('[') && formulas.EndsWith(']')) {
						var array = JArray.Parse(formulas);
						foreach (var item in array) {
							var r = engine.TryEvaluate(item["formula"].ToString(), null);
							if (r == null) { return Error(item["formula"].ToString() + " is error "); }
							result[item["name"].ToString()] = r;
						}
					} else if (formulas.StartsWith('{') && formulas.EndsWith('}')) {
						var item = JObject.Parse(formulas);
						var r = engine.TryEvaluate(item["formula"].ToString(), null);
						if (r == null) { return Error(item["formula"].ToString() + " is error "); }
						result[item["name"].ToString()] = r;
					} else {
						var r = engine.TryEvaluate(formulas, null);
						if (r == null) { return Error(formulas + " is error "); }
						result["result"] = r;
					}
				} else {
					if (formulas.StartsWith('[') && formulas.EndsWith(']')) {
						var array = JArray.Parse(formulas);
						foreach (var item in array) {
							var r = engine.TryEvaluate(item["formula"].ToString(), null);
							result[item["name"].ToString()] = r ?? item["formula"].ToString() + " is error ";
						}
					} else if (formulas.StartsWith('{') && formulas.EndsWith('}')) {
						var item = JObject.Parse(formulas);
						var r = engine.TryEvaluate(item["formula"].ToString(), null);
						result[item["name"].ToString()] = r ?? item["formula"].ToString() + " is error ";
					} else {
						var r = engine.TryEvaluate(formulas, null);
						result["result"] = r ?? formulas + " is error ";
					}
				}
				engine.Dispose();
				engine = null;
				return Success(result, status);
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		[IgnoreAntiforgeryToken]
		public IActionResult EvalWithMessages(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();

			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				if (work == null) { return Error("Please configure the project file information in appsettings.json ."); }
				FlowEngineEx engine = new FlowEngineEx(work, attachData, machines);
				var message = engine.GetErrorMessage(appCode, factoryCode, json, previous);
				return Success(message);
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		[IgnoreAntiforgeryToken]
		public IActionResult EvalFormulaDetailsWithError(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string addFormulas)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();

			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				if (work == null) { return Error("Please configure the project file information in appsettings.json ."); }
				FlowEngineEx engine = new FlowEngineEx(work, attachData, machines);
				engine.BuildTreeNodeWithError(appCode, factoryCode, json, previous);

				FormulaDetail result = engine.GetFormulaDetails(addFormulas);
				engine.Dispose();
				engine = null;
				return Success(result);
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		[IgnoreAntiforgeryToken]
		public IActionResult EvalFormulaDetails(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string formulas, string addFormulas)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			if (string.IsNullOrWhiteSpace(formulas)) { return Error("formulas is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();
			formulas = formulas.Trim();

			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				if (work == null) { return Error("Please configure the project file information in appsettings.json ."); }
				FlowEngineEx engine = new FlowEngineEx(work, attachData, machines);
				engine.BuildTreeNode(appCode, factoryCode, json, previous);
				engine.EvaluateInputNum();

				FormulaDetail result;
				if (formulas.StartsWith('[') && formulas.EndsWith(']')) {
					var array = JArray.Parse(formulas);
					Dictionary<string, string> dict = new Dictionary<string, string>();
					foreach (var item in array) {
						dict[item["name"].ToString()] = item["formula"].ToString();
					}
					result = engine.GetFormulaDetails(dict);
				} else if (formulas.StartsWith('{') && formulas.EndsWith('}')) {
					var item = JObject.Parse(formulas);
					result = engine.GetFormulaDetails(item["name"].ToString(), item["formula"].ToString());
				} else {
					result = engine.GetFormulaDetails("result", formulas, addFormulas);
				}
				engine.Dispose();
				engine = null;
				return Success(result);
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		[IgnoreAntiforgeryToken]
		public IActionResult EvalNodeConditionDetails(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string nodeId)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			if (string.IsNullOrWhiteSpace(nodeId)) { return Error("nodeId is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();

			FlowEngineEx engine = null;
			NodeConditionDetail result;
			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				if (work == null) { return Error("Please configure the project file information in appsettings.json ."); }
				try {
					engine = new FlowEngineEx(work, attachData, machines);
					engine.BuildTreeNode(appCode, factoryCode, json, previous);
					engine.EvaluateInputNum();
					result = engine.GetNodeConditionDetails(nodeId, factoryCode);

					return Success(result);
				} catch (Exception) {
				} finally {
					if (engine != null) {
						engine.Dispose();
						engine = null;
					}
				}
				engine = new FlowEngineEx(work, attachData, machines);
				engine.BuildTreeNodeWithError(appCode, factoryCode, json, previous);
				result = engine.GetNodeConditionDetails(nodeId, factoryCode);

				return Success(result);
			} catch (Exception ex) {
				return Error(ex.Message);
			} finally {
				if (engine != null) {
					engine.Dispose();
					engine = null;
				}
			}
		}

		[IgnoreAntiforgeryToken]
		public IActionResult EvalConditionDetails(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string nodeId, string name)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			if (string.IsNullOrWhiteSpace(nodeId)) { return Error("nodeId is null."); }
			if (string.IsNullOrWhiteSpace(name)) { return Error("name is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();
			name = name.Trim();

			FlowEngineEx engine = null;
			ConditionDetail result;
			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				if (work == null) { return Error("Please configure the project file information in appsettings.json ."); }
				try {
					engine = new FlowEngineEx(work, attachData, machines);
					engine.BuildTreeNode(appCode, factoryCode, json, previous);
					engine.EvaluateInputNum();
					result = engine.GetConditionDetails(nodeId, name);
					return Success(result);
				} catch (Exception) {
				} finally {
					if (engine != null) {
						engine.Dispose();
						engine = null;
					}
				}
				engine = new FlowEngineEx(work, attachData, machines);
				engine.BuildTreeNodeWithError(appCode, factoryCode, json, previous);
				result = engine.GetConditionDetails(nodeId, name);
				return Success(result);
			} catch (Exception ex) {
				return Error(ex.Message);
			} finally {
				if (engine != null) {
					engine.Dispose();
					engine = null;
				}
			}
		}

		[IgnoreAntiforgeryToken]
		public IActionResult EvalProcedureDetails(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string matches, bool all = false)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();

			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				if (work == null) { return Error("Please configure the project file information in appsettings.json ."); }
				FlowEngineEx engine = new FlowEngineEx(work, attachData, machines);
				engine.BuildTreeNode(appCode, factoryCode, json, previous);
				engine.EvaluateInputNum();

				List<FormulaMatch> formulaMatches = null;
				if (string.IsNullOrWhiteSpace(matches) == false) {
					formulaMatches = JsonUtil.DeserializeObject<List<FormulaMatch>>(matches);
				}
				var result = engine.GetProcedureDetail(formulaMatches, all);

				return Success(result);
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		[IgnoreAntiforgeryToken]
		public IActionResult EvalOptionalMachine(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string nodeId)
		{
			if (string.IsNullOrWhiteSpace(factoryCode)) { return Error("factoryCode is null."); }
			if (string.IsNullOrWhiteSpace(appCode)) { return Error("appCode is null."); }
			if (string.IsNullOrWhiteSpace(json)) { return Error("json is null."); }
			if (string.IsNullOrWhiteSpace(nodeId)) { return Error("nodeId is null."); }
			factoryCode = factoryCode.Trim();
			appCode = appCode.Trim();

			try {
				List<Setting_Machine> machines = null;
				if (string.IsNullOrWhiteSpace(machineInfos) == false) {
					machines = JsonUtil.DeserializeObject<List<Setting_Machine>>(machineInfos);
				}
				var work = GetProjectWork();
				if (work == null) { return Error("Please configure the project file information in appsettings.json ."); }
				FlowEngineEx engine = new FlowEngineEx(work, attachData, machines);
				engine.BuildTreeNode(appCode, factoryCode, json, previous);

				var result = engine.GetOptionalMachine(factoryCode, nodeId);
				return Success(result);
			} catch (Exception ex) {
				return Error(ex.Message);
			}
		}

		#endregion 计算公式、获取路径、获取公式步骤、获取公式条件、备选机械
	}
}