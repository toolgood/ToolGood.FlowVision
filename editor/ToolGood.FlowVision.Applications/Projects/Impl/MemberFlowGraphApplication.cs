using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm2;
using ToolGood.Algorithm2.Internals;
using ToolGood.FlowVision.Applications.Members.Impl;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.FlowVision.Datas;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Flow;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowVision.Dtos.Projects.Dtos;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowVision.Applications.Projects.Impl
{
	public class MemberFlowGraphApplication : ApplicationBase, IMemberFlowGraphApplication
	{
		#region 更新厂区名称,机械名称 工艺名称

		/// <summary>
		/// 图片 更新，更新厂区名称,机械名称 工艺名称
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async ValueTask<bool> GraphUpdate(Request<MemberIdDto> request)
		{
			if (request == null) { throw new ArgumentNullException(nameof(request)); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.ProjectId == null) { throw new ArgumentException("Data is null"); }

			var helper = GetWriteHelper();

			#region machineDtos procedureDtos

			var factories = await helper.Select_Async<DbFactory>("where mainMemberId=@0 and projectId=@1 and isDelete=0", request.MainMemberId, request.Data.ProjectId.Value);
			var machineDtos = await helper.Select_Async<FactoryMachineDto>("where mainMemberId=@0 and projectId=@1 and isDelete=0", request.MainMemberId, request.Data.ProjectId.Value);
			var procedureDtos = await helper.Select_Async<FactoryProcedureDto>("where mainMemberId=@0 and projectId=@1 and isDelete=0", request.MainMemberId, request.Data.ProjectId.Value);
			foreach (var machine in machineDtos) {
				var fact = factories.Where(q => q.Id == machine.FactoryId).FirstOrDefault();
				if (fact != null) {
					machine.Factory = fact.SimplifyName;
				}
			}

			foreach (var procedureDto in procedureDtos) {
				var fs = procedureDto.FactoryIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
				HashSet<string> names = new HashSet<string>();
				foreach (var f in fs) {
					var fact = factories.Where(q => q.Id.ToString() == f).FirstOrDefault();
					if (fact != null) { names.Add(fact.SimplifyName); }
				}
				if (names.Count == factories.Count) {
					procedureDto.Factorys = "所有厂区";
				} else {
					procedureDto.Factorys = string.Join(",", names);
				}
			}

			#endregion machineDtos procedureDtos

			var app = await helper.FirstOrDefault_Async<DbApp>("where mainMemberId=@0 and projectId=@1 and id=@2 and isDelete=0", request.MainMemberId, request.Data.ProjectId.Value, request.Data.AppId.Value);
			var appFlow = await helper.FirstOrDefault_Async<DbAppFlow>("where mainMemberId=@0 and projectId=@1 and AppCode=@2 and isDelete=0", request.MainMemberId, request.Data.ProjectId.Value, app.Code);
			if (appFlow == null) return true;
			var json = JObject.Parse(appFlow.FlowString);
			var array = json["cells"] as JArray;
			foreach (var item in array) {
				if (item["shape"].ToSafeString() == "procedure") {
					var code = item["data"]["procedure"].ToSafeString();
					var procedureDto = procedureDtos.FirstOrDefault(q => q.Code == code);
					if (procedureDto != null) {
						item["attrs"]["text"]["text"] = procedureDto.Name + '\n' + procedureDto.Factorys;
					}
					var machines = item["data"]["machines"] as JArray;
					if (machines != null) {
						foreach (var machine in machines) {
							var name = machine["name"].ToString();
							var machineDto = machineDtos.FirstOrDefault(q => q.Name == name);
							if (machineDto == null) { continue; }
							machine["factory"] = machineDto.Factory;
							machine["factoryCode"] = machineDto.FactoryCode;
							machine["machineName"] = machineDto.MachineName;
							machine["machineCategoryName"] = machineDto.MachineCategoryName;
						}
					}
				}
			}
			appFlow.FlowString = json.ToString(Newtonsoft.Json.Formatting.None);
			await helper.Save_Async(appFlow);

			return true;
		}

		#endregion 更新厂区名称,机械名称 工艺名称

		#region GraphUpdateFormulas 更新公式

		/// <summary>
		/// 更新公式
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async ValueTask<bool> GraphUpdateFormulas(Request<MemberIdDto> request)
		{
			if (request == null) { throw new ArgumentNullException(nameof(request)); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.ProjectId == null) { throw new ArgumentException("Data is null"); }

			var helper = GetWriteHelper();
			var formulas = await helper.Select_Async<DbProjectFormula>(" where mainMemberId=@0 and projectId=@1 and isDelete=0 and IsReferenceCode=1", request.MainMemberId, request.Data.ProjectId.Value);
			var dict = new Dictionary<string, string>();
			foreach (var formula in formulas) {
				dict[formula.Name] = formula.Formula;
			}
			var dbs = await helper.Select_Async<DbAppFlow>("where AppCode in (select AppCode from flow_app where mainMemberId=@0 and projectId=@1 and isDelete=0)", request.MainMemberId, request.Data.ProjectId.Value);

			foreach (var db in dbs) {
				var json = JObject.Parse(db.FlowString);
				var array = json["cells"] as JArray;
				foreach (var cell in array) {
					if (cell["data"] == null) { continue; }
					var data = cell["data"];
					if (data["checkFormula"] != null) {
						data["checkFormula"] = ReplaceFormulas(data["checkFormula"].ToString(), dict);
					}
					if (data["settingFormula"] != null) {
						var settingFormulas = data["settingFormula"] as JArray;
						for (int i = 0; i < settingFormulas.Count; i++) {
							var settingFormula = JObject.Parse(settingFormulas[i].ToString());
							if (settingFormula["conditions"] != null) {
								var conditions = settingFormula["conditions"] as JArray;
								foreach (var condition in conditions) {
									condition["condition"] = ReplaceFormulas(condition["condition"].ToString(), dict);
									condition["formula"] = ReplaceFormulas(condition["formula"].ToString(), dict);
								}
							}
							settingFormula["defaultFormula"] = ReplaceFormulas(settingFormula["defaultFormula"].ToString(), dict);
							settingFormulas[i] = settingFormula.ToString(Newtonsoft.Json.Formatting.None);
							settingFormula = null;
						}
					}
					if (data["inputFormula"] != null) {
						var inputFormulas = data["inputFormula"] as JArray;
						foreach (var inputFormula in inputFormulas) {
							inputFormula["condition"] = ReplaceFormulas(inputFormula["condition"].ToString(), dict);
							inputFormula["formula"] = ReplaceFormulas(inputFormula["formula"].ToString(), dict);
						}
					}
					if (data["machines"] != null) {
						var machines = data["machines"] as JArray;
						foreach (var machine in machines) {
							machine["condition"] = ReplaceFormulas(machine["condition"].ToString(), dict);
						}
					}
				}
				db.FlowString = json.ToString(Newtonsoft.Json.Formatting.None);
				json = null;
			}
			using (var tran = helper.UseTransaction()) {
				foreach (var db in dbs) {
					await helper.Save_Async(db);
				}
				tran.Complete();
			}
			return true;
		}

		private string ReplaceFormulas(string exp, Dictionary<string, string> dict)
		{
			if (string.IsNullOrEmpty(exp)) { return exp; }

			var diyName = AlgorithmEngineHelper.GetDiyNames(exp);
			StringBuilder stringBuilder = new StringBuilder();
			var index = 0;
			for (int i = 0; i < diyName.Parameters.Count; i++) {
				var p = diyName.Parameters[i];
				if (dict.ContainsKey(p.Name)) {
					while (index < p.Start) { stringBuilder.Append(exp[index++]); }
					stringBuilder.Append(dict[p.Name]);
					index = p.End + 1;
				}
			}
			while (index < exp.Length) { stringBuilder.Append(exp[index++]); }
			return stringBuilder.ToString();
		}

		#endregion GraphUpdateFormulas 更新公式

		#region 导出

		public async Task<ProjectWork> Exports(int mainMemberId, int projectId)
		{
			ProjectInfo project = await GetProject(mainMemberId, projectId);
			project.AppList = await LoadProjectApp(mainMemberId, projectId);
			foreach (var appInfo in project.AppList) {
				if (appInfo.Cells == null || appInfo.Cells.Count == 0) { continue; }
				if (appInfo.BuildFlow() == false) throw new Exception($"{appInfo.App.Name}没有开始节点!");
				appInfo.SetProcedure(project.ProcedureList, project.MachineList); //设置工艺 并判断 节点是否被使用
				if (appInfo.CheckEndNodeOrErrorNode() == false) throw new Exception($"{appInfo.App.Name}流程没有关闭!");
			}

			string message = null;
			foreach (var appInfo in project.AppList) {
				if (CheckAppFormula(project, appInfo, ref message) == false) {
					throw new Exception(message);
				}
			}
			return ToProjectWork(project);
		}

		#endregion 导出

		#region private GetProject LoadProjectApp

		private async Task<ProjectInfo> GetProject(int mainMemberId, int projectId)
		{
			var helper = GetReadHelper();
			ProjectInfo project = new ProjectInfo();

			#region machineDtos procedureDtos

			var dbProject = await helper.FirstOrDefault_Async<DbProject>(projectId);
			var formulas = await helper.Select_Async<DbProjectFormula>("where mainMemberId=@0 and projectId=@1 and isDelete=0 and IsReferenceCode=0", mainMemberId, projectId);
			var factories = await helper.Select_Async<DbFactory>("where mainMemberId=@0 and projectId=@1 and isDelete=0", mainMemberId, projectId);
			var machineDtos = await helper.Select_Async<FactoryMachineDto>("where mainMemberId=@0 and projectId=@1 and isDelete=0", mainMemberId, projectId);
			var procedureDtos = await helper.Select_Async<FactoryProcedureDto>("where mainMemberId=@0 and projectId=@1 and isDelete=0", mainMemberId, projectId);
			var procedureItems = await helper.Select_Async<FactoryProcedureItemDto>("where mainMemberId=@0 and projectId=@1 and isDelete=0 and Used=1", mainMemberId, projectId);
			var ProjectDatas = await helper.Select_Async<DbProjectData>("where mainMemberId=@0 and projectId=@1 and isDelete=0", mainMemberId, projectId);

			foreach (var item in machineDtos) {
				var fact = factories.Where(q => q.Id == item.FactoryId).FirstOrDefault();
				if (fact != null) {
					item.FactoryCode = fact.Code;
					item.Factory = fact.SimplifyName;
				}
			}
			foreach (var item in procedureItems) {
				var fact = factories.Where(q => q.Id == item.FactoryId).FirstOrDefault();
				if (fact != null) {
					item.FactoryCode = fact.Code;
					item.Factory = fact.SimplifyName;
				}
			}
			foreach (var procedureDto in procedureDtos) {
				var fs = procedureDto.FactoryIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
				List<string> names = new List<string>();
				foreach (var f in fs) {
					var fact = factories.Where(q => q.Id.ToString() == f).FirstOrDefault();
					if (fact != null) { names.Add(fact.SimplifyName); }
				}
				procedureDto.Factorys = string.Join(",", names);
				procedureDto.Items = procedureItems.Where(q => q.ProcedureId == procedureDto.Id).ToList();
			}

			#endregion machineDtos procedureDtos

			project.FactoryList = factories;
			project.MachineList = machineDtos;
			project.ProcedureList = procedureDtos;
			project.Project = dbProject;
			project.FormulaList = formulas;
			project.DataList = ProjectDatas;

			return project;
		}

		private async Task<List<ProjectAppInfo>> LoadProjectApp(int mainMemberId, int projectId)
		{
			var helper = GetReadHelper();
			var result = new List<ProjectAppInfo>();
			var apps = await helper.Select_Async<DbApp>("where isDelete=0 and mainMemberId=@0 and projectId=@1 ", mainMemberId, projectId);
			foreach (var app in apps) {
				try {
					var a = await LoadProjectApp(mainMemberId, projectId, app.Id);
					result.Add(a);
				} catch (Exception ex) {
					throw new Exception($"{app.Name},{ex.Message}");
				}
			}
			return result;
		}

		private async Task<ProjectAppInfo> LoadProjectApp(int mainMemberId, int projectId, int appId)
		{
			var helper = GetReadHelper();
			var projectAppInfo = new ProjectAppInfo();
			projectAppInfo.App = await helper.FirstOrDefault_Async<DbApp>("where mainMemberId=@0 and projectId=@1 and id=@2 and isDelete=0", mainMemberId, projectId, appId);
			projectAppInfo.InputList = await helper.Select_Async<DbAppInput>("where mainMemberId=@0 and projectId=@1 and appIds like @2 and isDelete=0", mainMemberId, projectId, "%," + appId + ",%");
			projectAppInfo.InitValueList = await helper.Select_Async<DbAppInitValue>("where mainMemberId=@0 and projectId=@1 and appIds like @2 and isDelete=0", mainMemberId, projectId, "%," + appId + ",%");

			var appFlow = await helper.FirstOrDefault_Async<DbAppFlow>("where mainMemberId=@0 and projectId=@1 and appCode=@2 and isDelete=0", mainMemberId, projectId, projectAppInfo.App.Code);
			if (appFlow == null) return projectAppInfo;
			projectAppInfo.Cells = new List<CellDto>();

			var json = JObject.Parse(appFlow.FlowString);
			var array = json["cells"] as JArray;
			foreach (var item in array) {
				var cell = CellDto.Parse(item);
				if (cell != null) {
					projectAppInfo.Cells.Add(cell);
				}
			}
			if (projectAppInfo.Cells.Count > 0) {
				projectAppInfo.BuildFlow();
			}
			return projectAppInfo;
		}

		#endregion private GetProject LoadProjectApp

		#region private ToProjectWork

		private ProjectWork ToProjectWork(ProjectInfo projectInfo)
		{
			ProjectWork work = new ProjectWork() {
				Name = projectInfo.Project.Name,
				Code = projectInfo.Project.Code,
				ExcelIndex = projectInfo.Project.ExcelIndex,
				NumberRequired = projectInfo.Project.NumberRequired == 1,
				Distance = projectInfo.Project.Distance,
				Area = projectInfo.Project.Area,
				Volume = projectInfo.Project.Volume,
				Mass = projectInfo.Project.Mass,
				FactoryList = new Dictionary<string, FactoryWork>(),
				FactoryMachineList = new Dictionary<string, FactoryMachineWork>(),
				FactoryProcedureList = new Dictionary<string, FactoryProcedureWork>(),
				FormulaList = new Dictionary<string, string>(),
				DataList = new Dictionary<string, string>(),
				AppList = new Dictionary<string, AppWork>(),
			};

			#region FactoryList

			foreach (var item in projectInfo.FactoryList) {
				work.FactoryList[item.Code] = new FactoryWork() {
					Code = item.Code,
					Name = item.SimplifyName,
				};
			}

			#endregion FactoryList

			#region MachineList

			foreach (var item in projectInfo.MachineList) {
				FactoryMachineWork machineWork = new FactoryMachineWork() {
					Factory = item.Factory,
					MachineCode = item.MachineCode,
					MachineCategoryCode = item.MachineCategoryCode,
					MachineCategoryName = item.MachineCategoryName,
					MachineName = item.MachineName,
					CheckFormula = item.CheckFormula,
				};
				work.FactoryMachineList[item.Name] = machineWork;
			}

			#endregion MachineList

			#region ProcedureList

			foreach (var item in projectInfo.ProcedureList) {
				FactoryProcedureWork procedureWork = new FactoryProcedureWork() {
					Name = item.Name,
					Category = item.Category,
					CheckFormula = item.CheckFormula,
					Code = item.Code,
					Items = new Dictionary<string, FactoryProcedureItemWork>()
				};
				if (item.Items != null) {
					foreach (var it in item.Items) {
						FactoryProcedureItemWork itemWork = new FactoryProcedureItemWork() {
							Category = it.Category,
							CategoryCode = it.CategoryCode,
							Code = it.Code,
							Name = it.Name
						};
						if (procedureWork.Items.ContainsKey(it.FactoryCode) == false) {
							procedureWork.Items[it.FactoryCode] = itemWork;
						}
					}
				}
				work.FactoryProcedureList[item.Code] = procedureWork;
			}

			#endregion ProcedureList

			#region FormulaList

			foreach (var item in projectInfo.FormulaList) {
				work.FormulaList[item.Name] = item.Formula;
			}

			#endregion FormulaList

			#region DataList
			foreach (var item in projectInfo.DataList) {
				var jo = JObject.Parse(item.Data);
				work.DataList[item.Name] = jo.ToString(Newtonsoft.Json.Formatting.None);
			}
			#endregion

			#region AppList

			foreach (var item in projectInfo.AppList) {
				if (item.Cells == null || item.Start == null) { continue; }

				AppWork appWork = new AppWork() {
					Code = item.App.Code,
					Name = item.App.Name,
					InputList = new List<AppInputWork>(),
					InitValueList = new List<AppInitValueWork>(),
					AllNodeWork = new Dictionary<string, NodeWork>(),
				};

				#region InputList

				foreach (var input in item.InputList) {
					appWork.InputList.Add(new AppInputWork() {
						DefaultValue = input.DefaultValue,
						UseDefaultValue = input.UseDefaultValue,
						CheckInput = input.CheckInput,
						ErrorMessage = input.ErrorMessage,
						InputName = input.InputName,
						Unit = input.Unit,
						IsRequired = input.IsRequired,
						InputType = ParseInputType(input.InputType)
					});
				}

				#endregion InputList

				#region InitValueList

				foreach (var initValue in item.InitValueList) {
					appWork.InitValueList.Add(new AppInitValueWork() {
						Name = initValue.Name,
						InputType = ParseInputType(initValue.InputType),
						ErrorMessage = initValue.ErrorMessage,
						IsThrowError = initValue.IsThrowError,
						Conditions = JsonUtil.DeserializeObject<List<SettingFormulaItemWork>>(initValue.SettingFormula)
					});
				}

				#endregion InitValueList

				foreach (var cell in item.Cells) {
					if (cell.IsUsed == false) { continue; }
					var w = To(cell);
					if (w != null) {
						appWork.AllNodeWork[w.Id] = w;
					}
				}
				// 空的节点
				HashSet<string> ids = new HashSet<string>();
				foreach (var cell in item.Cells) {
					if (cell is EdgeDto edge) {
						ids.Add(edge.SourceCell);
						ids.Add(edge.TargetCell);
					}
				}
				work.AppList[appWork.Code] = appWork;
			}

			#endregion AppList

			return work;
		}

		private ToolGood.FlowWork.Flows.InputType ParseInputType(string type)
		{
			if (type.Equals("number", StringComparison.OrdinalIgnoreCase)) {
				return ToolGood.FlowWork.Flows.InputType.Number;
			} else if (type.Equals("String", StringComparison.OrdinalIgnoreCase)) {
				return ToolGood.FlowWork.Flows.InputType.String;
			} else if (type.Equals("Bool", StringComparison.OrdinalIgnoreCase)) {
				return ToolGood.FlowWork.Flows.InputType.Bool;
			} else if (type.Equals("Date", StringComparison.OrdinalIgnoreCase)) {
				return ToolGood.FlowWork.Flows.InputType.Date;
			} else if (type.Equals("List", StringComparison.OrdinalIgnoreCase)) {
				return ToolGood.FlowWork.Flows.InputType.List;
			}
			throw new Exception("ParseInputType methon is error.");
		}

		private NodeWork To(CellDto cell)
		{
			if (cell is EdgeDto) { return null; }
			if (cell.NodeType == Dtos.Flow.CellType.Comment) { return null; }
			var node = (NodeDto)cell;
			var work = To2(node);
			if (work == null) { return null; }
			work.NextNodeIds = new Dictionary<string, List<string>>();
			foreach (var item in node.NextNodes) {
				List<string> ids = new List<string>();
				foreach (var nd in item.Value.OrderBy(q => q.Order)) {
					ids.Add(nd.Id);
				}
				work.NextNodeIds.Add(item.Key, ids);
			}
			return work;
		}

		private NodeWork To2(NodeDto node)
		{
			if (node is StartFlowDto start) {
				return new StartFlowWork() {
					Id = start.Id,
					Label = start.Label.Split(new char[] { '\r', '\n' })[0],
					Layer = start.Layer,
					SettingFormula = To(start.SettingFormula),
					Comment = start.Comment,
				};
			} else if (node is EndFlowDto end) {
				return new EndFlowWork() {
					Id = end.Id,
					Label = end.Label.Split(new char[] { '\r', '\n' })[0],
					Layer = end.Layer,
					SettingFormula = To(end.SettingFormula),
					Comment = end.Comment,
				};
			} else if (node is ErrorFlowDto error) {
				return new ErrorFlowWork() {
					Id = error.Id,
					Label = error.Label.Split(new char[] { '\r', '\n' })[0],
					Layer = error.Layer,
					ErrorMessage = error.ErrorMessage,
					CheckFormula = error.CheckFormula,
					Comment = error.Comment,
				};
			} else if (node is ProcedureFlowDto procedure) {
				return new ProcedureFlowWork() {
					Id = procedure.Id,
					Label = procedure.Label.Split(new char[] { '\r', '\n' })[0],
					Layer = procedure.Layer,
					CheckFormula = procedure.CheckFormula,
					CheckType = (ToolGood.FlowWork.Flows.CheckType)(int)procedure.CheckType,
					InputFormula = To(procedure.InputFormula),
					InputName = procedure.InputName,
					NumberType = (FlowWork.Flows.InputNumberType)(int)procedure.NumberType,
					IsSubsidiaryCount = procedure.IsSubsidiaryCount,
					SettingFormula = To(procedure.SettingFormula),
					Procedure = procedure.Procedure,
					MachineRequired = procedure.MachineRequired,
					Machines = To(procedure.Machines),
					Comment = procedure.Comment,
				};
			} else if (node is JumpFlowDto jump) {
				return new JumpFlowWork() {
					Id = jump.Id,
					Label = jump.Label.Split(new char[] { '\r', '\n' })[0],
					Layer = jump.Layer,
					CheckFormula = jump.CheckFormula,
					InputName = jump.InputName,
					SettingFormula = To(jump.SettingFormula),
					Comment = jump.Comment,
				};
			} else if (node is CustomFlowDto custom) {
				return new CustomFlowWork() {
					Id = custom.Id,
					Label = custom.Label.Split(new char[] { '\r', '\n' })[0],
					Layer = custom.Layer,
					CheckFormula = custom.CheckFormula,
					Names = custom.Names,
					Script = custom.Script,
					Comment = custom.Comment,
				};
			} else if (node is MergeFlowDto merge) {
				return new MergeFlowWork() {
					Id = merge.Id,
					Label = merge.Label.Split(new char[] { '\r', '\n' })[0],
					Layer = merge.Layer,
					SettingFormula = To(merge.SettingFormula),
					Comment = merge.Comment,
				};
			} else if (node is StatusFlowDto status) {
				return new StatusFlowWork() {
					Id = status.Id,
					Label = status.Label.Split(new char[] { '\r', '\n' })[0],
					Layer = status.Layer,
					SettingFormula = To(status.SettingFormula),
					Status = status.Status,
					StatusCheckFormula = status.StatusCheckFormula,
					CheckFormula = status.CheckFormula,
					Comment = status.Comment,
				};
			}
			return null;
		}

		private List<SettingFormulaWork> To(List<SettingFormulaDto> dtos)
		{
			List<SettingFormulaWork> result = new List<SettingFormulaWork>();
			if (dtos == null) { return result; }
			foreach (var dto in dtos) {
				result.Add(To(dto));
			}
			return result;
		}

		private SettingFormulaWork To(SettingFormulaDto dto)
		{
			return new SettingFormulaWork() {
				DefaultFormula = dto.DefaultFormula,
				Name = dto.Name,
				Type = (ToolGood.FlowWork.Flows.InputType)(int)dto.Type,
				Conditions = To(dto.Conditions),
				Comment = dto.Comment,
			};
		}

		private List<SettingFormulaItemWork> To(List<SettingFormulaItem> dtos)
		{
			List<SettingFormulaItemWork> result = new List<SettingFormulaItemWork>(dtos.Count);
			if (dtos != null) {
				foreach (var dto in dtos) {
					result.Add(To(dto));
				}
			}
			return result;
		}

		private SettingFormulaItemWork To(SettingFormulaItem dto)
		{
			return new SettingFormulaItemWork() {
				Condition = dto.Condition,
				Formula = dto.Formula
			};
		}

		private Dictionary<string, List<ProcedureFlowMachineInfo>> To(List<ProcedureFlowDto.MachineInfo> info)
		{
			Dictionary<string, List<ProcedureFlowMachineInfo>> result = new Dictionary<string, List<ProcedureFlowMachineInfo>>();
			foreach (var dto in info) {
				if (dto.IsUsed == false) { continue; }
				List<ProcedureFlowMachineInfo> list;
				if (result.TryGetValue(dto.FactoryCode, out list) == false) {
					list = new List<ProcedureFlowMachineInfo>();
					result[dto.FactoryCode] = list;
				}
				list.Add(To(dto));
			}
			return result;
		}

		private ProcedureFlowMachineInfo To(ProcedureFlowDto.MachineInfo info)
		{
			return new ProcedureFlowMachineInfo() {
				Name = info.Name,
				Condition = info.Condition,
				CheckType = (ToolGood.FlowWork.Flows.CheckType)(int)info.CheckType,
			};
		}

		#endregion private ToProjectWork

		#region private 检测审核 公式 与 参数

		private bool CheckAppFormula(ProjectInfo projectInfo, ProjectAppInfo appInfo, ref string message)
		{
			HashSet<string> varNames = new HashSet<string>(); //项目可用的公式名称
			varNames.Add("厂区");
			varNames.Add("厂区编号");
			varNames.Add("数量");
			varNames.Add("出量");
			varNames.Add("上一流程");

			HashSet<string> varNames2 = new HashSet<string>(); //被用到的项目公式
			AlgorithmEngine engine = new AlgorithmEngine();
			// 1、检测 项目公式是否正确
			if (CheckProjectFormula(projectInfo.FormulaList, varNames, engine, ref message) == false) { return false; }
			// 2、 检测 输入公式是否正确
			if (CheckInputFormula(appInfo, varNames, engine, ref message) == false) { return false; }
			// 3、检测 输入公式参数 是否存在
			if (CheckInputVarName(appInfo, varNames, varNames2, ref message) == false) { return false; }
			// 4、 检测 输入公式是否正确
			if (CheckInitValueFormula(appInfo, varNames, engine, ref message) == false) { return false; }
			// 5、检测 输入公式参数 是否存在
			if (CheckInitValueVarName(appInfo, varNames, varNames2, ref message) == false) { return false; }
			// 6、检测 公式是否正确
			if (CheckFormula(appInfo, varNames, engine, ref message) == false) { return false; }
			// 7、检测 公式参数 是否存在
			if (CheckVarName(appInfo, varNames, varNames2, ref message) == false) { return false; }
			// 8、检测 项目公式参数 是否存在
			if (CheckProjectFormulaVarName(projectInfo.FormulaList, varNames, varNames2, ref message) == false) { return false; }

			// 88检测 输入值的默认值是否正确

			return true;
		}

		private bool CheckInputFormula(ProjectAppInfo appInfo, HashSet<string> varNames, AlgorithmEngine engine, ref string message)
		{
			#region InputList

			foreach (var input in appInfo.InputList) {
				varNames.Add(input.InputName);
				if (string.IsNullOrEmpty(input.CheckInput) == false) {
					if (engine.Parse(input.CheckInput) == false) {
						message = $"流程：{appInfo.App.Name}，输入项:{input.InputName},[检查输入]出错了!";
						return false;
					}
				}
			}

			#endregion InputList

			return true;
		}

		private bool CheckInitValueFormula(ProjectAppInfo appInfo, HashSet<string> varNames, AlgorithmEngine engine, ref string message)
		{
			#region InitValueList

			foreach (var initValue in appInfo.InitValueList) {
				varNames.Add(initValue.Name);
				if (string.IsNullOrEmpty(initValue.SettingFormula) == false) {
					var items = JsonUtil.DeserializeObject<List<SettingFormulaItem>>(initValue.SettingFormula);
					if (CheckSettingFormulaItems(items, engine, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，初始值：{initValue.Name}{message}";
						return false;
					}
				}
			}

			#endregion InitValueList

			return true;
		}

		private bool CheckSettingFormulaItems(List<SettingFormulaItem> items, AlgorithmEngine engine, ref string message)
		{
			for (int i = 0; i < items.Count; i++) {
				var item = items[i];
				if (string.IsNullOrEmpty(item.Condition)) {
					if (i < items.Count - 1) {
						message = $"第{i + 1}个[条件]为空!";
						return false;
					}
				} else {
					if (engine.Parse(item.Condition) == false) {
						message = $"第{i + 1}个[条件]出错了!";
						return false;
					}
				}
				if (string.IsNullOrEmpty(item.Formula)) {
					message = $"第{i + 1}个[公式]为空!";
					return false;
				} else {
					if (engine.Parse(item.Formula) == false) {
						message = $"第{i + 1}个[公式]出错了!";
						return false;
					}
				}
			}
			return true;
		}

		private bool CheckFormula(ProjectAppInfo appInfo, HashSet<string> varNames, AlgorithmEngine engine, ref string message)
		{
			if (appInfo.Cells == null) { return true; }
			foreach (var cell in appInfo.Cells) {
				//if (cell.IsUsed == false) { continue; }
				if (cell is StartFlowDto start) {

					#region start

					if (CheckFormula(start.SettingFormula, varNames, engine, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{start.Id},{start.Label.Split('\n')[0]},[配置公式]{message}";
						return false;
					}

					#endregion start
				} else if (cell is ProcedureFlowDto procedure) {

					#region procedure

					varNames.Add(procedure.InputName);
					if (string.IsNullOrEmpty(procedure.CheckFormula) == false && engine.Parse(procedure.CheckFormula) == false) {
						message = $"流程：{appInfo.App.Name}，id:{procedure.Id},{procedure.Label.Split('\n')[0]},[检测公式]出错了!";
						return false;
					}
					if (CheckSettingFormulaItems(procedure.InputFormula, engine, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{procedure.Id},{procedure.Label.Split('\n')[0]},[入量公式]出错了，{message}";
						return false;
					}
					if (CheckFormula(procedure.SettingFormula, varNames, engine, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{procedure.Id},{procedure.Label.Split('\n')[0]},[配置公式]{message}";
						return false;
					}
					if (procedure.FactoryProcedure == null) {
						message = $"流程：{appInfo.App.Name}，id:{procedure.Id},{procedure.Label.Split('\n')[0]},未选择[工艺]";
						return false;
					}
					if (string.IsNullOrEmpty(procedure.FactoryProcedure.CheckFormula) == false) {
						if (engine.Parse(procedure.FactoryProcedure.CheckFormula) == false) {
							message = $"工艺:[{procedure.FactoryProcedure.Code}]{procedure.FactoryProcedure.Name.Split('\n')[0]},[检测公式]出错了!";
							return false;
						}
					}
					foreach (var machineInfo in procedure.Machines) {
						if (string.IsNullOrEmpty(machineInfo.Condition) == false) {
							if (engine.Parse(machineInfo.Condition) == false) {
								message = $"工艺:[{procedure.FactoryProcedure.Code}]{procedure.FactoryProcedure.Name.Split('\n')[0]},机器[{machineInfo.Name}]的[条件]出错了!";
								return false;
							}
						}
						if (machineInfo.FactoryMachine != null) {
							if (string.IsNullOrEmpty(machineInfo.FactoryMachine.CheckFormula) == false) {
								if (engine.Parse(machineInfo.FactoryMachine.CheckFormula) == false) {
									message = $"机械:[{machineInfo.FactoryMachine.Name}][条件]出错了!";
									return false;
								}
							}
						}
					}

					#endregion procedure
				} else if (cell is CustomFlowDto custom) {

					#region custom

					foreach (var item in custom.Names) {
						varNames.Add(item);
					}

					#endregion custom
				} else if (cell is JumpFlowDto jump) {

					#region jump

					varNames.Add(jump.InputName);
					if (string.IsNullOrEmpty(jump.CheckFormula) == false && engine.Parse(jump.CheckFormula) == false) {
						message = $"流程：{appInfo.App.Name}，id:{jump.Id},{jump.Label.Split('\n')[0]},[检测公式]出错了!";
						return false;
					}
					if (CheckFormula(jump.SettingFormula, varNames, engine, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{jump.Id},{jump.Label.Split('\n')[0]},[配置公式]{message}";
						return false;
					}

					#endregion jump
				} else if (cell is StatusFlowDto status) {

					#region status

					if (string.IsNullOrEmpty(status.Status)) {
						message = $"流程：{appInfo.App.Name}，id:{status.Id},{status.Label.Split('\n')[0]},[状态码]为空!";
						return false;
					}
					if (string.IsNullOrEmpty(status.CheckFormula) == false && engine.Parse(status.CheckFormula) == false) {
						message = $"流程：{appInfo.App.Name}，id:{status.Id},{status.Label.Split('\n')[0]},[通过公式]出错了!";
						return false;
					}
					if (string.IsNullOrEmpty(status.StatusCheckFormula) == false && engine.Parse(status.StatusCheckFormula) == false) {
						message = $"流程：{appInfo.App.Name}，id:{status.Id},{status.Label.Split('\n')[0]},[状态条件]出错了!";
						return false;
					}
					if (CheckFormula(status.SettingFormula, varNames, engine, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{status.Id},{status.Label.Split('\n')[0]},[配置公式]{message}";
						return false;
					}

					#endregion status
				} else if (cell is MergeFlowDto merge) {

					#region merge

					if (CheckFormula(merge.SettingFormula, varNames, engine, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{merge.Id},{merge.Label.Split('\n')[0]},[配置公式]{message}";
						return false;
					}

					#endregion merge
				} else if (cell is EndFlowDto end) {

					#region end

					if (CheckFormula(end.SettingFormula, varNames, engine, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{end.Id},{end.Label.Split('\n')[0]},[配置公式]{message}";
						return false;
					}

					#endregion end
				}
			}
			return true;
		}

		private bool CheckFormula(List<SettingFormulaDto> settingFormulas, HashSet<string> varNames, AlgorithmEngine engine, ref string message)
		{
			foreach (var settingFormula in settingFormulas) {
				varNames.Add(settingFormula.Name);
				for (int i = 0; i < settingFormula.Conditions.Count; i++) {
					var item = settingFormula.Conditions[i];
					if (string.IsNullOrEmpty(item.Condition)) {
						message = $"{settingFormula.Name}第{i + 1}个[条件]为空!";
						return false;
					} else {
						if (engine.Parse(item.Condition) == false) {
							message = $"{settingFormula.Name}第{i + 1}个[条件]出错了!";
							return false;
						}
					}
					if (string.IsNullOrEmpty(item.Formula)) {
						message = $"{settingFormula.Name}第{i + 1}个[公式]为空!";
						return false;
					} else {
						if (engine.Parse(item.Formula) == false) {
							message = $"{settingFormula.Name}第{i + 1}个[公式]出错了!";
							return false;
						}
					}
				}
				if (string.IsNullOrEmpty(settingFormula.DefaultFormula)) { message = $"{settingFormula.Name}[默认公式]为空"; return false; }
				if (engine.Parse(settingFormula.DefaultFormula) == false) { message = $"{settingFormula.Name}[默认公式]出错了"; return false; }
			}
			return true;
		}

		private bool CheckInputVarName(ProjectAppInfo appInfo, HashSet<string> varNames, HashSet<string> varNames2, ref string message)
		{
			DiyNameInfo names;

			#region InputList

			foreach (var input in appInfo.InputList) {
				if (string.IsNullOrWhiteSpace(input.CheckInput) == false) {
					names = AlgorithmEngineHelper.GetDiyNames(input.CheckInput);
					foreach (var parameter in names.Parameters) {
						varNames2.Add(parameter.Name);
						if (varNames.Add(parameter.Name)) {
							message = $"流程：{appInfo.App.Name}，输入项:{input.InputName},[检查输入]出错了!参数[{parameter.Name}]不存在！";
							return false;
						}
					}
				}
			}

			#endregion InputList

			return true;
		}

		private bool CheckInitValueVarName(ProjectAppInfo appInfo, HashSet<string> varNames, HashSet<string> varNames2, ref string message)
		{
			#region InitValueList

			foreach (var initValue in appInfo.InitValueList) {
				if (string.IsNullOrEmpty(initValue.SettingFormula) == false) {
					var items = JsonUtil.DeserializeObject<List<SettingFormulaItem>>(initValue.SettingFormula);
					if (CheckSettingFormulaItemsVarName(items, varNames, varNames2, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，初始值：{initValue.Name}{message}";
						return false;
					}
				}
			}

			#endregion InitValueList

			return true;
		}

		private bool CheckSettingFormulaItemsVarName(List<SettingFormulaItem> items, HashSet<string> varNames, HashSet<string> varNames2, ref string message)
		{
			DiyNameInfo names;
			for (int i = 0; i < items.Count; i++) {
				var item = items[i];
				if (string.IsNullOrEmpty(item.Condition) == false) {
					names = AlgorithmEngineHelper.GetDiyNames(item.Condition);
					foreach (var parameter in names.Parameters) {
						varNames2.Add(parameter.Name);
						if (varNames.Add(parameter.Name)) {
							message = $"第{i + 1}个[条件]出错了!参数[{parameter.Name}]不存在！";
							return false;
						}
					}
				}
				if (string.IsNullOrEmpty(item.Formula) == false) {
					names = AlgorithmEngineHelper.GetDiyNames(item.Formula);
					foreach (var parameter in names.Parameters) {
						varNames2.Add(parameter.Name);
						if (varNames.Add(parameter.Name)) {
							message = $"第{i + 1}个[公式]出错了!参数[{parameter.Name}]不存在！";
							return false;
						}
					}
				}
			}
			return true;
		}

		private bool CheckVarName(ProjectAppInfo appInfo, HashSet<string> varNames, HashSet<string> varNames2, ref string message)
		{
			if (appInfo.Cells == null) { return true; }
			DiyNameInfo names;
			foreach (var cell in appInfo.Cells) {
				//if (cell.IsUsed == false) { continue; }
				if (cell is StartFlowDto start) {

					#region start

					if (CheckVarName(start.SettingFormula, varNames, varNames2, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{start.Id},{start.Label.Split(',')[0]},[配置公式]{message}";
						return false;
					}

					#endregion start
				} else if (cell is ProcedureFlowDto procedure) {

					#region procedure

					if (string.IsNullOrEmpty(procedure.CheckFormula) == false) {
						names = AlgorithmEngineHelper.GetDiyNames(procedure.CheckFormula);
						if (CheckVarName(names, varNames, varNames2, ref message) == false) {
							message = $"流程：{appInfo.App.Name}，id:{procedure.Id},{procedure.Label.Split(',')[0]},[检测公式]出错了!{message}";
							return false;
						}
					}
					if (CheckSettingFormulaItemsVarName(procedure.InputFormula, varNames, varNames2, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{procedure.Id},{procedure.Label.Split('\n')[0]},[入量公式]出错了，{message}";
						return false;
					}
					if (CheckVarName(procedure.SettingFormula, varNames, varNames2, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{procedure.Id},{procedure.Label.Split(',')[0]},[配置公式]{message}";
						return false;
					}
					if (string.IsNullOrEmpty(procedure.FactoryProcedure.CheckFormula) == false) {
						names = AlgorithmEngineHelper.GetDiyNames(procedure.FactoryProcedure.CheckFormula);
						if (CheckVarName(names, varNames, varNames2, ref message) == false) {
							message = $"工艺:[{procedure.FactoryProcedure.Code}]{procedure.FactoryProcedure.Name.Split(',')[0]},[检测公式]出错了!{message}";
							return false;
						}
					}
					//foreach (var item in procedure.FactoryProcedure.Items) {
					//    if (string.IsNullOrEmpty(item.CheckFormula) == false) {
					//        names = AlgorithmEngineHelper.GetDiyNames(item.CheckFormula);
					//        if (CheckVarName(names, varNames, varNames2, ref message) == false) {
					//            message = $"工艺:[{procedure.FactoryProcedure.Code}]{procedure.FactoryProcedure.Name.Split(',')[0]},厂区{item.Factory}[检测公式]出错了!{message}";
					//            return false;
					//        }
					//    }
					//}

					#endregion procedure
				} else if (cell is JumpFlowDto jump) {

					#region jump

					if (string.IsNullOrEmpty(jump.CheckFormula) == false) {
						names = AlgorithmEngineHelper.GetDiyNames(jump.CheckFormula);
						if (CheckVarName(names, varNames, varNames2, ref message) == false) {
							message = $"流程：{appInfo.App.Name}，id:{jump.Id},{jump.Label.Split(',')[0]},[检测公式]出错了!{message}";
							return false;
						}
					}
					if (CheckVarName(jump.SettingFormula, varNames, varNames2, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{jump.Id},{jump.Label.Split(',')[0]},[配置公式] {message}";
						return false;
					}

					#endregion jump
				} else if (cell is StatusFlowDto status) {

					#region status

					if (CheckVarName(status.SettingFormula, varNames, varNames2, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{status.Id},{status.Label.Split(',')[0]},[配置公式] {message}";
						return false;
					}

					#endregion status
				} else if (cell is MergeFlowDto merge) {

					#region merge

					if (CheckVarName(merge.SettingFormula, varNames, varNames2, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{merge.Id},{merge.Label.Split(',')[0]},[配置公式] {message}";
						return false;
					}

					#endregion merge
				} else if (cell is EndFlowDto end) {

					#region end

					if (CheckVarName(end.SettingFormula, varNames, varNames2, ref message) == false) {
						message = $"流程：{appInfo.App.Name}，id:{end.Id},{end.Label.Split(',')[0]},[配置公式]{message}";
						return false;
					}

					#endregion end
				} else if (cell is CustomFlowDto custom) {
					foreach (var name in custom.GetNames) {
						if (varNames.Add(name)) {
							message = $"流程：{appInfo.App.Name}，id:{custom.Id},{custom.Label.Split(',')[0]},参数[{name}]不存在！";
							return false;
						}
					}
				}
			}
			return true;
		}

		private bool CheckVarName(List<SettingFormulaDto> settingFormulas, HashSet<string> varNames, HashSet<string> varNames2, ref string message)
		{
			DiyNameInfo names = null;
			foreach (var settingFormula in settingFormulas) {
				foreach (var condition in settingFormula.Conditions) {
					names = AlgorithmEngineHelper.GetDiyNames(condition.Condition);
					if (CheckVarName(names, varNames, varNames2, ref message) == false) {
						message = $"{settingFormula.Name}[条件]出错了!{message}";
						return false;
					}
					names = AlgorithmEngineHelper.GetDiyNames(condition.Formula);
					if (CheckVarName(names, varNames, varNames2, ref message) == false) {
						message = $"{settingFormula.Name}[公式]出错了!{message}";
						return false;
					}
				}
				names = AlgorithmEngineHelper.GetDiyNames(settingFormula.DefaultFormula);
				if (CheckVarName(names, varNames, varNames2, ref message) == false) {
					message = $"{settingFormula.Name}[默认公式]出错了!{message}";
					return false;
				}
			}
			return true;
		}

		private bool CheckVarName(DiyNameInfo names, HashSet<string> varNames, ref string message)
		{
			foreach (var parameter in names.Parameters) {
				if (varNames.Add(parameter.Name)) {
					message = $"参数[{parameter.Name}]不存在！";
					return false;
				}
			}
			return true;
		}

		private bool CheckVarName(DiyNameInfo names, HashSet<string> varNames, HashSet<string> varNames2, ref string message)
		{
			foreach (var parameter in names.Parameters) {
				varNames2.Add(parameter.Name);
				if (varNames.Add(parameter.Name)) {
					message = $"参数[{parameter.Name}]不存在！";
					return false;
				}
			}
			return true;
		}

		private bool CheckProjectFormula(List<DbProjectFormula> dict, HashSet<string> varNames, AlgorithmEngine engine, ref string message)
		{
			foreach (var item in dict) {
				varNames.Add(item.Name);
				if (string.IsNullOrEmpty(item.Formula)) {
					message = $"{item.Name}[公式]为空!";
					return false;
				} else {
					if (engine.Parse(item.Formula) == false) {
						message = $"{item.Name}[公式]出错了!";
						return false;
					}
				}
			}
			return true;
		}

		private bool CheckProjectFormulaVarName(List<DbProjectFormula> dict, HashSet<string> varNames, HashSet<string> varNames2, ref string message)
		{
			var checkNames = new HashSet<string>();
			foreach (var item in varNames2) { checkNames.Add(item); }
			Dictionary<string, DbProjectFormula> dic = dict.ToDictionary(q => q.Name, q => q);

			while (checkNames.Count > 0) {
				var NewCheckNames = new HashSet<string>();
				foreach (var checkName in checkNames) {
					if (dic.TryGetValue(checkName, out DbProjectFormula item)) {
						var names = AlgorithmEngineHelper.GetDiyNames(item.Formula);
						if (CheckVarName(names, varNames, ref message) == false) {
							message = $"[项目公式]{item.Name}[公式]出错了!{message}";
							return false;
						}
						foreach (var name in names.Parameters) {
							if (varNames2.Add(name.Name)) {
								NewCheckNames.Add(name.Name);
							}
						}
					}
				}
				checkNames = NewCheckNames;
			}
			return true;
		}

		#endregion private 检测审核 公式 与 参数

		#region 枚举检测条件
		public async Task<List<string>> EnumerationConditions(int mainMemberId, int projectId)
		{
			var projectWork = await Exports(mainMemberId, projectId);
			projectWork.Init();

			List<string> result = new List<string>();
			foreach (var app in projectWork.AppList) {
				List<AppTestJsonDto> testJsonDtos = new List<AppTestJsonDto>();
				foreach (var item in app.Value.InputList) {
					AppTestJsonDto appTestJsonDto = new AppTestJsonDto();
					appTestJsonDto.Name = item.InputName;
					appTestJsonDto.Type = (AppTestJsonType)(int)item.InputType;
					appTestJsonDto.IsRequired = item.IsRequired;
					testJsonDtos.Add(appTestJsonDto);
				}
				string names = string.Join("|", testJsonDtos.Where(q => q.Type != AppTestJsonType.Bool).Select(q => q.Name).ToList());
				foreach (var item in app.Value.InputList) {
					ExtractFeature2(item.CheckInput, names, testJsonDtos);
				}

				var start = app.Value.AllNodeWork.First(q => q.Value.NodeType == FlowWork.Flows.CellType.Start);
				EnumerationConditions(start.Value, names, testJsonDtos);

				result.Add("-----------------------------------------------------------------");
				result.Add($"--- {app.Value.Name} / {app.Key}");
				result.Add("-----------------------------------------------------------------");
				result.Add("    \"factoryCode\": [\"" + String.Join("\",\"", projectWork.FactoryList.Keys) + "\"],");
				result.Add("    \"appCode\": [\"" + app.Key + "\"],");
				result.Add("    \"json\": {");
				foreach (var appTestJsonDto in testJsonDtos) {
					if (appTestJsonDto.Type == AppTestJsonType.Number) {
						appTestJsonDto.BuildNumber();
						if (appTestJsonDto.IsRequired) {
							result.Add("        \"" + appTestJsonDto.Name + "\": [" + String.Join(", ", appTestJsonDto.TextList) + "],");
						} else {
							result.Add("        \"" + appTestJsonDto.Name + "\": [None, " + String.Join(", ", appTestJsonDto.TextList) + "],");
						}
					} else if (appTestJsonDto.Type == AppTestJsonType.Bool) {
						if (appTestJsonDto.IsRequired) {
							result.Add("        \"" + appTestJsonDto.Name + "\": [True, False],");
						} else {
							result.Add("        \"" + appTestJsonDto.Name + "\": [None, True, False],");
						}
					} else {
						if (appTestJsonDto.IsRequired) {
							result.Add("        \"" + appTestJsonDto.Name + "\": [\"" + String.Join("\", \"", appTestJsonDto.TextList) + "\"],");
						} else {
							result.Add("        \"" + appTestJsonDto.Name + "\": [None, \"" + String.Join("\", \"", appTestJsonDto.TextList) + "\"],");
						}
					}
				}
				result.Add("    }");
			}

			return result;
		}
		public void EnumerationConditions(NodeWork work, string names, List<AppTestJsonDto> testJsonDtos)
		{
			if (work is StartFlowWork start) {
				EnumerationConditions(work.NextNodes, names, testJsonDtos);
				ExtractFeature2(start.SettingFormula, names, testJsonDtos);
			} else if (work is EndFlowWork end) {
				ExtractFeature2(end.SettingFormula, names, testJsonDtos);
			} else if (work is ErrorFlowWork error) {
				ExtractFeature2(error.CheckFormula, names, testJsonDtos);
			} else if (work is ProcedureFlowWork procedure) {
				ExtractFeature2(procedure.CheckFormula, names, testJsonDtos);
				ExtractFeature2(procedure.FactoryProcedure.CheckFormula, names, testJsonDtos);
				ExtractFeature2(procedure.SettingFormula, names, testJsonDtos);

				if (procedure.MachineRequired) {
					foreach (var item in procedure.Machines) {
						foreach (var item2 in item.Value) {
							if (item2.CheckType == FlowWork.Flows.CheckType.Replace) {
								ExtractFeature2(item2.Condition, names, testJsonDtos);
							} else {
								ExtractFeature2(item2.Condition, names, testJsonDtos);
								ExtractFeature2(item2.FactoryMachine.CheckFormula, names, testJsonDtos);
							}
						}
					}
				}
				EnumerationConditions(procedure.NextNodes, names, testJsonDtos);
			} else if (work is CustomFlowWork custom) {
				ExtractFeature2(custom.CheckFormula, names, testJsonDtos);
				EnumerationConditions(custom.NextNodes, names, testJsonDtos);
			} else if (work is JumpFlowWork jump) {
				ExtractFeature2(jump.CheckFormula, names, testJsonDtos);
				ExtractFeature2(jump.SettingFormula, names, testJsonDtos);

				EnumerationConditions(jump.NextNodes, names, testJsonDtos);
			} else if (work is MergeFlowWork merge) {
				ExtractFeature2(merge.SettingFormula, names, testJsonDtos);

				EnumerationConditions(merge.NextNodes, names, testJsonDtos);
			} else if (work is StatusFlowWork status) {
				ExtractFeature2(status.CheckFormula, names, testJsonDtos);
				ExtractFeature2(status.StatusCheckFormula, names, testJsonDtos);
				ExtractFeature2(status.SettingFormula, names, testJsonDtos);
				EnumerationConditions(status.NextNodes, names, testJsonDtos);
			}
		}

		private void ExtractFeature2(List<SettingFormulaWork> settingFormulas, string names, List<AppTestJsonDto> appTestJsons)
		{
			if (settingFormulas == null) { return; }
			foreach (var settingFormula in settingFormulas) {
				foreach (var item in settingFormula.Conditions) {
					ExtractFeature2(item.Condition, names, appTestJsons);
				}
			}
		}


		private void EnumerationConditions(Dictionary<string, List<NodeWork>> next, string names, List<AppTestJsonDto> testJsonDtos)
		{
			foreach (var item in next) {
				foreach (var item2 in item.Value) {
					EnumerationConditions(item2, names, testJsonDtos);
				}
			}
		}
		private void ExtractFeature2(string text, string names, List<AppTestJsonDto> appTestJsons)
		{
			if (string.IsNullOrEmpty(text)) { return; }
			var trees = ConditionTree.Parse(text);
			AddTree(trees, names, appTestJsons);
		}
		private void AddTree(ConditionTree tree, string names, List<AppTestJsonDto> appTestJsons)
		{
			if (tree.Type == Algorithm2.Enums.ConditionTreeType.And) {
				AddTree(tree.Nodes[0], names, appTestJsons);
				AddTree(tree.Nodes[1], names, appTestJsons);
			} else if (tree.Type == Algorithm2.Enums.ConditionTreeType.Or) {
				AddTree(tree.Nodes[0], names, appTestJsons);
				AddTree(tree.Nodes[1], names, appTestJsons);
			} else if (tree.Type == Algorithm2.Enums.ConditionTreeType.String) {
				ExtractFeature(tree.ConditionString, names, appTestJsons);
			}
		}

		private void ExtractFeature(string text, string names, List<AppTestJsonDto> appTestJsons)
		{
			if (string.IsNullOrEmpty(text)) { return; }
			text = text.Trim();


			var ms = Regex.Matches(text, $@"^\{{(.*?)\}}\.has[(（]({names})[)）]$", RegexOptions.IgnoreCase);
			foreach (Match m in ms) {
				var t = m.Groups[1].Value;
				var name = m.Groups[2].Value;
				var testJsonDto = appTestJsons.FirstOrDefault(q => q.Name == name);
				if (testJsonDto == null) { continue; }
				if (testJsonDto.Type == AppTestJsonType.Bool) { continue; }

				var sp = t.Split(',', StringSplitOptions.RemoveEmptyEntries);
				foreach (var s in sp) {
					var temp = s.Replace("\"", "").Replace("'", "").Replace("“", "").Replace("”", "").Replace("‘", "").Replace("’", "").Trim();
					testJsonDto.TextList.Add(temp);
				}
			}
			ms = Regex.Matches(text, $@"({names}).in[(（](.*)[)）]", RegexOptions.IgnoreCase);
			foreach (Match m in ms) {
				var t = m.Groups[2].Value;
				var name = m.Groups[1].Value;
				var testJsonDto = appTestJsons.FirstOrDefault(q => q.Name == name);
				if (testJsonDto == null) { continue; }
				if (testJsonDto.Type == AppTestJsonType.Bool) { continue; }

				var sp = t.Split(',', StringSplitOptions.RemoveEmptyEntries);
				foreach (var s in sp) {
					var temp = s.Replace("\"", "").Replace("'", "").Replace("“", "").Replace("”", "").Replace("‘", "").Replace("’", "").Trim();
					testJsonDto.TextList.Add(temp);
				}
			}
			ms = Regex.Matches(text, $@"^({names})[ \t]*(===|==|=|!==|!=|<>|>=|<=|>|<)[ \t]*([^ ]*?)$", RegexOptions.IgnoreCase);
			foreach (Match m in ms) {
				var t = m.Groups[3].Value;
				var name = m.Groups[1].Value;
				var testJsonDto = appTestJsons.FirstOrDefault(q => q.Name == name);
				if (testJsonDto == null) { continue; }
				if (testJsonDto.Type == AppTestJsonType.Bool) { continue; }
				var temp = t.Replace("\"", "").Replace("'", "").Replace("“", "").Replace("”", "").Replace("‘", "").Replace("’", "").Trim();


				testJsonDto.TextList.Add(temp);
			}
			ms = Regex.Matches(text, $@"^([^ !=<>]*?)[ \t]*(===|==|=|!==|!=|<>|>=|<=|>|<)[ \t]*({names})$", RegexOptions.IgnoreCase);
			foreach (Match m in ms) {
				var t = m.Groups[1].Value;
				var name = m.Groups[3].Value;
				var testJsonDto = appTestJsons.FirstOrDefault(q => q.Name == name);
				if (testJsonDto == null) { continue; }
				if (testJsonDto.Type == AppTestJsonType.Bool) { continue; }
				var temp = t.Replace("\"", "").Replace("'", "").Replace("“", "").Replace("”", "").Replace("‘", "").Replace("’", "").Trim();

				testJsonDto.TextList.Add(temp);
			}
		}
		#endregion

		public async ValueTask<DateTime> GetTime()
		{
			var helper = GetReadHelper();
			var settingValue = await helper.FirstOrDefault_Async<DbSysSettingValue>("where Code='time' and IsDelete=0");
			if (settingValue != null) {
				if (DateTime.TryParse(settingValue.Value, out DateTime dt)) {
					return dt;
				}
			}
			return DateTime.Now;
		}

		public async ValueTask<DateTime> GetRealTime()
		{
			var dt = await GetTime();
			if (dt > DateTime.Now) {
				return dt;
			}
			return DateTime.Now;
		}


	}
}