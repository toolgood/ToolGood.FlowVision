using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text.RegularExpressions;
using ToolGood.Algorithm2;
using ToolGood.FlowVision.Applications.Members.Impl;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Projects.Dtos;

namespace ToolGood.FlowVision.Applications.Projects.Impl
{
	public class MemberFlowImportApplication : ApplicationBase, IMemberFlowImportApplication
	{
		private static Regex _inputNameRegex = new Regex("^[a-z\u3400-\u9fff_][0-9a-z\u3400-\u9fff_]*$", RegexOptions.IgnoreCase);
		private readonly IMemberFlowApplication _memberFlowApplication;

		public MemberFlowImportApplication(IMemberFlowApplication memberFlowApplication)
		{
			_memberFlowApplication = memberFlowApplication;
		}

		private bool CheckInputName(string name)
		{
			if (name == "数量" || name == "出量" || name == "厂区" || name == "厂区编号" || name == "上个流程") {
				return false;
			}
			if (_inputNameRegex.IsMatch(name) == false) {
				return false;
			}
			if (AlgorithmEngineHelper.IsKeywords(name)) {
				return false;
			}
			return true;
		}

		#region Project 项目

		public byte[] GetProjectTemplate()
		{
			IWorkbook workbook = new XSSFWorkbook();
			BuildProjectDictTemplate(workbook);
			BuildProjectFormulaTemplate(workbook);
			BuildFactoryTemplate(workbook);
			BuildFactoryMachineTemplate(workbook);
			BuildFactoryProcedureTemplate(workbook);
			BuildAppTemplate(workbook);
			BuildAppInputTemplate(workbook);
			BuildAppInitValueTemplate(workbook);
			using (var ms = new MemoryStream()) {
				workbook.Write(ms, false);
				workbook.Close();
				return ms.ToArray();
			}
		}

		public async ValueTask<bool> ImportProject(int projectId, Request<Stream> body)
		{
			IWorkbook workbook = null;
			try {
				workbook = WorkbookFactory.Create(body.Data);
				var helper = GetWriteHelper();

				using (var tran = helper.UseTransaction()) {
					await helper.Update_Async<DbProjectDict>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					await helper.Update_Async<DbProjectFormula>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					await helper.Update_Async<DbFactory>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					await helper.Update_Async<DbFactoryMachine>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					await helper.Update_Async<DbFactoryProcedure>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					await helper.Update_Async<DbFactoryProcedureItem>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					await helper.Update_Async<DbApp>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					await helper.Update_Async<DbAppInput>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					await helper.Update_Async<DbAppInitValue>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);
					//await helper.Update_Async<DbAppFlow>("set IsDelete=1,DeleteUserId=@2,DeleteTime=@3 where IsDelete=0 and ProjectId=@0 and MainMemberId=@1", projectId, body.MainMemberId, body.OperatorId, DateTime.Now);

					var projectDicts = ImportProjectDict(projectId, body, workbook);
					foreach (var item in projectDicts) helper.Insert(item);

					var projectFormulas = ImportProjectFormula(projectId, body, workbook);
					foreach (var item in projectFormulas) helper.Insert(item);

					var factories = ImportFactory(projectId, body, workbook);
					foreach (var item in factories) helper.Insert(item);

					var factoryMachines = ImportFactoryMachine(projectId, body, workbook, factories);
					foreach (var item in factoryMachines) helper.Insert(item);

					var factoryProcedures = ImportFactoryProcedure(projectId, body, workbook, factories);
					foreach (var item in factoryProcedures) {
						item.FactoryIds = ',' + String.Join(',', item.Items.Select(q => q.FactoryId)) + ',';
						helper.Insert(item);
						foreach (var item2 in item.Items) {
							item2.ProcedureId = item.Id;
							helper.Insert(item2);
						}
					}

					var apps = ImportApp(projectId, body, workbook);
					foreach (var item in apps) helper.Insert(item);

					var appInputs = ImportAppInput(projectId, body, workbook, apps);
					foreach (var item in appInputs) helper.Insert(item);

					var appInitValues = ImportAppInitValue(projectId, body, workbook, apps);
					foreach (var item in appInitValues) helper.Insert(item);

					var flows = await helper.Select_Async<DbAppFlow>("where MainMemberId=@1 and ProjectId=@0", projectId, body.MainMemberId);
					foreach (var item in apps) {
						if (flows.Any(q => q.AppCode == item.Code) == false) {
							helper.Insert(new DbAppFlow() {
								AppCode = item.Code,
								FlowString = "{\"cells\":[]}",
								MainMemberId = body.MainMemberId,
								ProjectId = projectId,
								CreateTime = DateTime.Now,
								CreateUserId = body.MainMemberId,
								ModifyTime = DateTime.Now,
								ModifyUserId = body.MainMemberId,
							});
						}
					}
					tran.Complete();
				}
				await _memberFlowApplication.AddProjectOperationLog(projectId, $"导入项目：", body);

				return true;
			} catch (Exception ex) {
				await _memberFlowApplication.AddProjectOperationLog(projectId, $"导入项目失败：" + ex.Message, body);
				body.Message = "导入项目失败：" + ex.Message;
				if (workbook != null) { workbook.Close(); }
				return false;
			}
		}

		#endregion Project 项目

		#region ProjectDict 项目字典

		private void BuildProjectDictTemplate(IWorkbook workbook)
		{
			var sh = workbook.CreateSheet("项目字典");
			var row = sh.CreateRow(0);
			int index = 0;
			row.CreateCell(index++).SetCellValue("分类");
			row.CreateCell(index++).SetCellValue("词汇");
			row.CreateCell(index++).SetCellValue("单位");
			row.CreateCell(index++).SetCellValue("备注");

			sh.Freeze(1, 0);
		}

		public List<DbProjectDict> ImportProjectDict(int projectId, Request<Stream> body, IWorkbook workbook)
		{
			var sh = workbook.GetSheet("项目字典");
			if (sh == null) { throw new Exception("未找到【项目字典】"); }

			List<DbProjectDict> projectDicts = new List<DbProjectDict>();
			for (int i = 1; i <= sh.PhysicalNumberOfRows; i++) {
				var row = sh.GetRow(i); if (row == null) { continue; }
				DbProjectDict db = new DbProjectDict() {
					CreateTime = DateTime.Now,
					CreateUserId = body.OperatorId,
					ModifyTime = DateTime.Now,
					ModifyUserId = body.OperatorId,
					MainMemberId = body.MainMemberId,
					ProjectId = projectId,

					Category = row.ReadString(0)?.Trim(),
					Name = row.ReadString(1)?.Trim(),
					Unit = row.ReadString(2)?.Trim(),
					Comment = row.ReadString(3)?.Trim(),
				};
				if (string.IsNullOrEmpty(db.Name)) { continue; }
				projectDicts.Add(db);
			}
			return projectDicts;
		}

		#endregion ProjectDict 项目字典

		#region ProjectFormula 项目公式

		private void BuildProjectFormulaTemplate(IWorkbook workbook)
		{
			var sh = workbook.CreateSheet("项目公式");
			var row = sh.CreateRow(0);
			int index = 0;
			row.CreateCell(index++).SetCellValue("分类");
			row.CreateCell(index++).SetCellValue("名称");
			row.CreateCell(index++).SetCellValue("公式");
			row.CreateCell(index++).SetCellValue("条件");
			row.CreateCell(index++).SetCellValue("备注");
			sh.Freeze(1, 0);
		}

		public List<DbProjectFormula> ImportProjectFormula(int projectId, Request<Stream> body, IWorkbook workbook)
		{
			var sh = workbook.GetSheet("项目公式");
			if (sh == null) { throw new Exception("未找到【项目公式】"); }

			List<DbProjectFormula> projectDicts = new List<DbProjectFormula>();
			HashSet<string> set = new HashSet<string>();
			for (int i = 1; i <= sh.PhysicalNumberOfRows; i++) {
				var row = sh.GetRow(i); if (row == null) { continue; }
				DbProjectFormula db = new DbProjectFormula() {
					CreateTime = DateTime.Now,
					CreateUserId = body.OperatorId,
					ModifyTime = DateTime.Now,
					ModifyUserId = body.OperatorId,
					MainMemberId = body.MainMemberId,
					ProjectId = projectId,
					IsReferenceCode = false,

					Category = row.ReadString(0)?.Trim(),
					Name = row.ReadString(1)?.Trim(),
					Formula = row.ReadString(2)?.Trim(),
					Comment = row.ReadString(4)?.Trim(),
				};
				if (string.IsNullOrEmpty(db.Name)) { continue; }
				//if (db.Name.Contains("数量") || db.Name.Contains("费用") || db.Name.Contains("单价") || db.Name.Contains("价格")) {
				//    db.IsReferenceCode = false;
				//}

				db.Name = db.Name.Replace("-", "_");
				if (CheckInputName(db.Name) == false) {
					throw new Exception($"[项目公式]第{i + 1}行[名称][{db.Name}]有问题！");
				}
				if (set.Add(db.Name) == false) {
					throw new Exception($"[项目公式]第{i + 1}行[名称][{db.Name}]有重复！");
				}
				var c = row.ReadString(3)?.Trim();
				if (string.IsNullOrEmpty(c) == false) {
					DbProjectFormula db2 = new DbProjectFormula() {
						CreateTime = DateTime.Now,
						CreateUserId = body.OperatorId,
						ModifyTime = DateTime.Now,
						ModifyUserId = body.OperatorId,
						MainMemberId = body.MainMemberId,
						ProjectId = projectId,
						IsReferenceCode = true,

						Category = db.Category,
						Name = "if" + db.Name,
						Formula = c,
						Comment = "",
					};
					db2.Name = db2.Name.Replace("-", "_");
					if (set.Add(db2.Name) == false) {
						throw new Exception($"[项目公式]第{i + 1}行[条件名称][{db2.Name}]有重复！");
					}
					projectDicts.Add(db2);
				}
				if (string.IsNullOrEmpty(db.Formula) == false) {
					projectDicts.Add(db);
				}
			}
			return projectDicts;
		}

		#endregion ProjectFormula 项目公式

		#region Factory 厂区

		private void BuildFactoryTemplate(IWorkbook workbook)
		{
			var sh = workbook.CreateSheet("厂区");
			var row = sh.CreateRow(0);
			int index = 0;
			row.CreateCell(index++).SetCellValue("分类");
			row.CreateCell(index++).SetCellValue("编码");
			row.CreateCell(index++).SetCellValue("简称");
			row.CreateCell(index++).SetCellValue("备注");
			sh.Freeze(1, 0);
		}

		public List<DbFactory> ImportFactory(int projectId, Request<Stream> body, IWorkbook workbook)
		{
			var sh = workbook.GetSheet("厂区");
			if (sh == null) { throw new Exception("未找到【厂区】"); }

			List<DbFactory> factorys = new List<DbFactory>();
			HashSet<string> codeSet = new HashSet<string>();
			HashSet<string> nameSet = new HashSet<string>();
			for (int i = 1; i <= sh.PhysicalNumberOfRows; i++) {
				var row = sh.GetRow(i); if (row == null) { continue; }
				DbFactory db = new DbFactory() {
					CreateTime = DateTime.Now,
					CreateUserId = body.OperatorId,
					ModifyTime = DateTime.Now,
					ModifyUserId = body.OperatorId,
					MainMemberId = body.MainMemberId,
					ProjectId = projectId,

					Category = row.ReadString(0)?.Trim(),
					Code = row.ReadString(1)?.Trim(),
					SimplifyName = row.ReadString(2)?.Trim(),
					Comment = row.ReadString(3)?.Trim(),
				};
				if (string.IsNullOrEmpty(db.Code)) { continue; }
				if (string.IsNullOrEmpty(db.SimplifyName)) { continue; }

				if (codeSet.Add(db.Code) == false)
					throw new Exception($"[厂区]第{i + 1}行[编码]有重复！");
				if (nameSet.Add(db.SimplifyName) == false)
					throw new Exception($"[厂区]第{i + 1}行[简称]有重复！");

				factorys.Add(db);
			}
			return factorys;
		}

		#endregion Factory 厂区

		#region FactoryMachine  厂区机械

		private void BuildFactoryMachineTemplate(IWorkbook workbook)
		{
			var sh = workbook.CreateSheet("厂区机械");
			var row = sh.CreateRow(0);
			int index = 0;
			row.CreateCell(index++).SetCellValue("分类");
			row.CreateCell(index++).SetCellValue("子分类");
			row.CreateCell(index++).SetCellValue("名称");
			row.CreateCell(index++).SetCellValue("厂区简称");

			row.CreateCell(index++).SetCellValue("机械编码");
			row.CreateCell(index++).SetCellValue("机械名称");
			row.CreateCell(index++).SetCellValue("分类编码");
			row.CreateCell(index++).SetCellValue("分类名称");

			row.CreateCell(index++).SetCellValue("检测公式");
			row.CreateCell(index++).SetCellValue("备注");
			row.CreateCell(index++).SetCellValue("排序");
			sh.Freeze(1, 0);
		}

		public List<FactoryMachineDto> ImportFactoryMachine(int projectId, Request<Stream> body, IWorkbook workbook, List<DbFactory> factoryList)
		{
			var sh = workbook.GetSheet("厂区机械");
			if (sh == null) { throw new Exception("未找到【厂区机械】"); }

			FactoryMachineDto pre = new FactoryMachineDto();
			List<FactoryMachineDto> factoryMachines = new List<FactoryMachineDto>();
			HashSet<string> codeSet = new HashSet<string>();
			HashSet<string> nameSet = new HashSet<string>();
			HashSet<string> codeSet2 = new HashSet<string>();
			for (int i = 1; i <= sh.PhysicalNumberOfRows; i++) {
				var row = sh.GetRow(i); if (row == null) { continue; }
				FactoryMachineDto db = new FactoryMachineDto() {
					CreateTime = DateTime.Now,
					CreateUserId = body.OperatorId,
					ModifyTime = DateTime.Now,
					ModifyUserId = body.OperatorId,
					MainMemberId = body.MainMemberId,
					ProjectId = projectId,

					Category = row.ReadString(0)?.Trim(),
					SubCategory = row.ReadString(1)?.Trim(),
					Name = row.ReadString(2)?.Trim(),
					FactoryCode = row.ReadString(3)?.Trim(),
					MachineCode = row.ReadString(4)?.Trim(),
					MachineName = row.ReadString(5)?.Trim(),
					MachineCategoryCode = row.ReadString(6)?.Trim(),
					MachineCategoryName = row.ReadString(7)?.Trim(),
					CheckFormula = row.ReadString(8)?.Trim(),
					Comment = row.ReadString(9)?.Trim(),
					OrderNum = row.ReadNumber(10, 10000),
				};
				if (string.IsNullOrEmpty(db.Category)) { continue; }
				if (string.IsNullOrEmpty(db.SubCategory)) { continue; }
				if (string.IsNullOrEmpty(db.Name)) { continue; }
				if (string.IsNullOrEmpty(db.FactoryCode)) { continue; }
				if (nameSet.Add(db.Name) == false) {
					throw new Exception($"[厂区机械]第{i + 1}行[名称]有重复！");
				}
				if (db.Name.Contains(',')) {
					throw new Exception($"[厂区机械]第{i + 1}行[名称]包含逗号！");
				}

				var factory = factoryList.Where(q => q.SimplifyName == db.FactoryCode).FirstOrDefault();
				if (factory == null) throw new Exception($"[厂区机械]第{i + 1}行[厂区简称]有问题！");
				db.FactoryId = factory.Id;

				if (db.MachineCode != null && db.MachineName != null) {
					if (codeSet2.Add(db.FactoryCode + '|' + db.MachineCategoryCode + '|' + db.MachineCategoryName + '|' + db.MachineCode + '|' + db.MachineName) == false)
						throw new Exception($"[厂区机械]第{i + 1}行[厂区-分类-编码-机械名称]有重复！");
				}
				factoryMachines.Add(db);
			}
			return factoryMachines;
		}

		#endregion FactoryMachine  厂区机械

		#region FactoryProcedure 厂区工艺

		private void BuildFactoryProcedureTemplate(IWorkbook workbook)
		{
			var sh = workbook.CreateSheet("厂区工艺");
			var row = sh.CreateRow(0);
			int index = 0;
			row.CreateCell(index++).SetCellValue("分类");
			row.CreateCell(index++).SetCellValue("编码");
			row.CreateCell(index++).SetCellValue("工艺名称");
			row.CreateCell(index++).SetCellValue("检测公式");
			row.CreateCell(index++).SetCellValue("备注");
			row.CreateCell(index).SetCellValue("厂区");

			var max = index;
			row = sh.CreateRow(1);
			row.CreateCell(index++).SetCellValue("厂区简称");
			row.CreateCell(index++).SetCellValue("工艺编码");
			row.CreateCell(index++).SetCellValue("工艺名称");
			row.CreateCell(index++).SetCellValue("分类编码");
			row.CreateCell(index++).SetCellValue("分类名称");
			row.CreateCell(index++).SetCellValue("备注");

			for (int i = 0; i < max; i++) {
				sh.MergedRow(0, i, 2);
			}
			sh.MergedColumn(0, max, 4);
			sh.Freeze(2, 0);
		}

		public List<FactoryProcedureDto> ImportFactoryProcedure(int projectId, Request<Stream> body, IWorkbook workbook, List<DbFactory> factoryList)
		{
			var sh = workbook.GetSheet("厂区工艺");
			if (sh == null) { throw new Exception("未找到【厂区工艺】"); }

			FactoryProcedureDto pre = new FactoryProcedureDto();
			List<FactoryProcedureDto> factoryProcedureList = new List<FactoryProcedureDto>();
			HashSet<string> codeSet = new HashSet<string>();
			HashSet<string> nameSet = new HashSet<string>();
			HashSet<string> codeSet2 = new HashSet<string>();
			for (int i = 2; i <= sh.PhysicalNumberOfRows; i++) {
				var row = sh.GetRow(i); if (row == null) { continue; }
				FactoryProcedureDto db = new FactoryProcedureDto() {
					CreateTime = DateTime.Now,
					CreateUserId = body.OperatorId,
					ModifyTime = DateTime.Now,
					ModifyUserId = body.OperatorId,
					MainMemberId = body.MainMemberId,
					ProjectId = projectId,

					Category = row.ReadString(0)?.Trim(),
					Code = row.ReadString(1)?.Trim(),
					Name = row.ReadString(2)?.Trim(),
					CheckFormula = row.ReadString(3)?.Trim(),
					Comment = row.ReadString(4)?.Trim(),
				};
				if (string.IsNullOrEmpty(db.Code)) { continue; }
				if (string.IsNullOrEmpty(db.Name)) { continue; }

				if (/*pre.Code == db.Code &&*/ pre.Category == db.Category && pre.Name == db.Name) {
					db = pre;
				} else {
					db.Items = new List<FactoryProcedureItemDto>();
					factoryProcedureList.Add(db);

					if (codeSet.Add(db.Code) == false)
						throw new Exception($"[厂区工艺]第{i + 1}行[编码]有重复！");
					if (nameSet.Add(db.Name) == false)
						throw new Exception($"[厂区工艺]第{i + 1}行[简称]有重复！");
					pre = db;
				}

				#region 厂区工艺标记

				var temp = new FactoryProcedureItemDto() {
					Factory = row.ReadString(5)?.Trim(),
					Code = row.ReadString(6)?.Trim(),
					Name = row.ReadString(7)?.Trim(),
					CategoryCode = row.ReadString(8)?.Trim(),
					Category = row.ReadString(9)?.Trim(),
					Comment = row.ReadString(10)?.Trim(),
				};
				if (temp.Factory != null || temp.Code != null || temp.Name != null || temp.CategoryCode != null || temp.Category != null || temp.Comment != null) {
					var factorys = temp.Factory.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var factoryname in factorys) {
						var itemDto2 = new FactoryProcedureItemDto() {
							CreateTime = DateTime.Now,
							CreateUserId = body.OperatorId,
							ModifyTime = DateTime.Now,
							ModifyUserId = body.OperatorId,
							MainMemberId = body.MainMemberId,
							ProjectId = projectId,

							Factory = factoryname,
							Code = row.ReadString(6)?.Trim(),
							Name = row.ReadString(7)?.Trim(),
							CategoryCode = row.ReadString(8)?.Trim(),
							Category = row.ReadString(9)?.Trim(),
							Comment = row.ReadString(10)?.Trim(),
						};
						db.Items.Add(itemDto2);
						var factory = factoryList.Where(q => q.SimplifyName == factoryname).FirstOrDefault();
						if (factory == null) throw new Exception($"[厂区工艺]第{i + 1}行[厂区简称]有问题！");
						itemDto2.FactoryId = factory.Id;
						if (itemDto2.Code != null && itemDto2.Name != null) {
							if (codeSet2.Add(itemDto2.Factory + '|' + itemDto2.Category + '|' + itemDto2.Code + '|' + itemDto2.Name) == false)
								throw new Exception($"[厂区工艺]第{i + 1}行[厂区-分类-编码-机械名称]有重复！");
							if (itemDto2.Category != itemDto2.CategoryCode && codeSet2.Add(itemDto2.Factory + '|' + itemDto2.CategoryCode + '|' + itemDto2.Code + '|' + itemDto2.Name) == false)
								throw new Exception($"[厂区工艺]第{i + 1}行[厂区-分类编码-编码-机械名称]有重复！");
						}
					}
				}

				#endregion 厂区工艺标记
			}
			return factoryProcedureList;
		}

		#endregion FactoryProcedure 厂区工艺

		#region App 流程信息

		private void BuildAppTemplate(IWorkbook workbook)
		{
			var sh = workbook.CreateSheet("流程信息");
			var row = sh.CreateRow(0);
			int index = 0;
			row.CreateCell(index++).SetCellValue("编码");
			row.CreateCell(index++).SetCellValue("名称");
			row.CreateCell(index++).SetCellValue("备注");

			sh.Freeze(1, 0);
		}

		private List<DbApp> ImportApp(int projectId, Request<Stream> body, IWorkbook workbook)
		{
			var sh = workbook.GetSheet("流程信息");
			if (sh == null) { throw new Exception("未找到【流程信息】"); }

			List<DbApp> apps = new List<DbApp>();
			HashSet<string> codeSet = new HashSet<string>();
			HashSet<string> nameSet = new HashSet<string>();
			for (int i = 1; i <= sh.PhysicalNumberOfRows; i++) {
				var row = sh.GetRow(i); if (row == null) { continue; }
				DbApp db = new DbApp() {
					CreateTime = DateTime.Now,
					CreateUserId = body.OperatorId,
					ModifyTime = DateTime.Now,
					ModifyUserId = body.OperatorId,
					MainMemberId = body.MainMemberId,
					ProjectId = projectId,

					Code = row.ReadString(0)?.Trim(),
					Name = row.ReadString(1)?.Trim(),
					Comment = row.ReadString(2)?.Trim(),
				};
				if (string.IsNullOrEmpty(db.Code)) { continue; }
				if (string.IsNullOrEmpty(db.Name)) { continue; }

				if (codeSet.Add(db.Code) == false)
					throw new Exception($"[流程信息]第{i + 1}行[编码]有重复！");
				if (nameSet.Add(db.Name) == false)
					throw new Exception($"[流程信息]第{i + 1}行[名称]有重复！");

				apps.Add(db);
			}
			return apps;
		}

		#endregion App 流程信息

		#region AppInput 输入项

		private void BuildAppInputTemplate(IWorkbook workbook)
		{
			var sh = workbook.CreateSheet("输入项");
			var row = sh.CreateRow(0);
			int index = 0;
			row.CreateCell(index++).SetCellValue("流程名称");
			row.CreateCell(index++).SetCellValue("输入项");
			row.CreateCell(index++).SetCellValue("输入类型");
			row.CreateCell(index++).SetCellValue("单位");
			row.CreateCell(index++).SetCellValue("检查输入");
			row.CreateCell(index++).SetCellValue("必填");
			row.CreateCell(index++).SetCellValue("使用默认值");
			row.CreateCell(index++).SetCellValue("默认值");
			row.CreateCell(index++).SetCellValue("抛错信息");
			row.CreateCell(index++).SetCellValue("备注");

			sh.Freeze(1, 0);
		}

		public List<AppInputDto> ImportAppInput(int projectId, Request<Stream> body, IWorkbook workbook, List<DbApp> apps)
		{
			var sh = workbook.GetSheet("输入项");
			var trues = new List<string>() { "true", "是", "y", "1", "yes" };

			List<AppInputDto> appInputs = new List<AppInputDto>();
			HashSet<string> nameSet = new HashSet<string>();
			for (int i = 1; i <= sh.PhysicalNumberOfRows; i++) {
				var row = sh.GetRow(i); if (row == null) { continue; }
				AppInputDto db = new AppInputDto() {
					CreateTime = DateTime.Now,
					CreateUserId = body.OperatorId,
					ModifyTime = DateTime.Now,
					ModifyUserId = body.OperatorId,
					MainMemberId = body.MainMemberId,
					ProjectId = projectId,

					Apps = row.ReadString(0)?.Trim(),
					InputName = row.ReadString(1)?.Trim(),
					InputType = row.ReadString(2)?.Trim(),
					Unit = row.ReadString(3)?.Trim(),
					CheckInput = row.ReadString(4)?.Trim(),
					IsRequired = trues.Contains(row.ReadString(5)?.Trim()),
					UseDefaultValue = trues.Contains(row.ReadString(6)?.Trim()),
					DefaultValue = row.ReadString(7)?.Trim(),
					ErrorMessage = row.ReadString(8)?.Trim(),
					Comment = row.ReadString(9)?.Trim(),
				};
				if (string.IsNullOrEmpty(db.InputName)) { continue; }

				if (CheckInputName(db.InputName) == false) {
					throw new Exception($"[输入项]第{i + 1}行[名称][{db.InputName}]有问题！");
				}

				var appNames = db.Apps.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
				List<int> appids = new List<int>();
				foreach (var appName in appNames) {
					var app = apps.Where(q => q.Name == appName.Trim()).FirstOrDefault();
					if (app == null) throw new Exception($"[输入项]第{i + 1}行[流程名称]有问题！");
					appids.Add(app.Id);
				}
				db.AppIds = "," + string.Join(",", appids.ToArray()) + ",";

				foreach (var item in appids) {
					if (nameSet.Add(db.InputName + "|" + item) == false)
						throw new Exception($"[输入项]第{i + 1}行[输入项]有重复！");
				}
				appInputs.Add(db);
			}
			return appInputs;
		}

		#endregion AppInput 输入项

		#region AppInitValue 初始值

		private void BuildAppInitValueTemplate(IWorkbook workbook)
		{
			var sh = workbook.CreateSheet("初始值");
			var row = sh.CreateRow(0);
			int index = 0;
			row.CreateCell(index++).SetCellValue("流程名称");
			row.CreateCell(index++).SetCellValue("变量名称");
			row.CreateCell(index++).SetCellValue("值类型");
			row.CreateCell(index++).SetCellValue("检测公式");
			row.CreateCell(index++).SetCellValue("公式");
			row.CreateCell(index++).SetCellValue("是否抛出错误");
			row.CreateCell(index++).SetCellValue("抛错信息");
			row.CreateCell(index++).SetCellValue("备注");

			sh.Freeze(1, 0);
		}

		private List<AppInitValueDto> ImportAppInitValue(int projectId, Request<Stream> body, IWorkbook workbook, List<DbApp> apps)
		{
			var sh = workbook.GetSheet("初始值");
			if (sh == null) { throw new Exception("未找到【初始值】"); }
			var trues = new List<string>() { "true", "是", "y", "1", "yes" };

			List<AppInitValueDto> appInputs = new List<AppInitValueDto>();
			HashSet<string> nameSet = new HashSet<string>();
			for (int i = 1; i <= sh.PhysicalNumberOfRows; i++) {
				var row = sh.GetRow(i); if (row == null) { continue; }
				AppInitValueDto db = new AppInitValueDto() {
					CreateTime = DateTime.Now,
					CreateUserId = body.OperatorId,
					ModifyTime = DateTime.Now,
					ModifyUserId = body.OperatorId,
					MainMemberId = body.MainMemberId,
					ProjectId = projectId,

					Apps = row.ReadString(0)?.Trim(),
					Name = row.ReadString(1)?.Trim(),
					InputType = row.ReadString(2)?.Trim(),
					IsThrowError = trues.Contains(row.ReadString(5)?.Trim()),
					ErrorMessage = row.ReadString(6)?.Trim(),
					Comment = row.ReadString(7)?.Trim(),
				};
				if (string.IsNullOrEmpty(db.Name)) { continue; }

				if (CheckInputName(db.Name) == false) {
					throw new Exception($"[初始值]第{i + 1}行[名称][{db.Name}]有问题！");
				}

				var txt1 = (row.ReadString(3)?.Trim() ?? "");
				var txt2 = (row.ReadString(4)?.Trim() ?? "");
				List<FormulaItem> formulaItems = new List<FormulaItem>();
				formulaItems.Add(new FormulaItem() {
					condition = txt1,
					formula = txt2
				});
				db.SettingFormula = JsonUtil.SerializeObject(formulaItems);

				var appNames = db.Apps.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
				List<int> appids = new List<int>();
				foreach (var appName in appNames) {
					var app = apps.Where(q => q.Name == appName.Trim()).FirstOrDefault();
					if (app == null) throw new Exception($"[初始值]第{i + 1}行[流程名称]有问题！");
					appids.Add(app.Id);
				}
				db.AppIds = "," + string.Join(",", appids.ToArray()) + ",";
				foreach (var item in appids) {
					if (nameSet.Add(db.Name + "|" + item) == false)
						throw new Exception($"[初始值]第{i + 1}行[变量名称]有重复！");
				}
				appInputs.Add(db);
			}
			return appInputs;
		}

		public class FormulaItem
		{
			public string condition { get; internal set; }
			public string formula { get; internal set; }
		}

		#endregion AppInitValue 初始值
	}
}