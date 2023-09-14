using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowVision.Dtos.Projects.Dtos;
using ToolGood.ReadyGo3;

namespace ToolGood.FlowVision.Applications.Projects
{
	public interface IMemberFlowApplication
	{
		#region Project 项目

		/// <summary>
		/// 修改 项目
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> AddProject(Request<DbProject> request);

		/// <summary>
		/// 修改 项目
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditProject(Request<DbProject> request);

		/// <summary>
		/// 获取 所有项目
		/// </summary>
		/// <returns></returns>
		Task<List<DbProject>> GetProjectAll(int mainMemberId);

		/// <summary>
		/// 获取 项目 页
		/// </summary>
		/// <returns></returns>
		Task<Page<DbProject>> GetProjectPage(Request<GetProjectListDto> request);

		/// <summary>
		/// 获取 项目
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbProject> GetProjectById(int mainMemberId, int id);

		#endregion Project 项目

		#region ProjecLog 项目日志

		/// <summary>
		/// 添加 项目日志
		/// </summary>
		/// <param name="operationLog"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddProjectOperationLog(int projectId, string msg, IRequest request);

		/// <summary>
		/// 获取 项目 页
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<Page<ProjectLogDto>> GetProjectLogPage(Request<GetProjectLogListDto> request);

		/// <summary>
		/// 获取 项目日志
		/// </summary>
		/// <param name="mainMemberId"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbProjectLog> GetProjectOperationLogById(int mainMemberId, int id);

		#endregion ProjecLog 项目日志

		#region ProjectDict 项目字典

		/// <summary>
		/// 添加 项目字典
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddProjectDict(Request<DbProjectDict> request);

		/// <summary>
		/// 修改 项目字典
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditProjectDict(Request<DbProjectDict> request);

		/// <summary>
		/// 删除 项目字典
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteProjectDict(Request<MemberIdDto> request);

		/// <summary>
		/// 获取所有 项目字典
		/// </summary>
		/// <returns></returns>
		Task<List<DbProjectDict>> GetProjectDictAll(int mainMemberId, int projectId);

		/// <summary>
		/// 获取  项目字典 页
		/// </summary>
		/// <returns></returns>
		Task<Page<DbProjectDict>> GetProjectDictPage(Request<GetProjectDictListDto> request);

		/// <summary>
		/// 获取 项目字典
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbProjectDict> GetProjectDictById(int mainMemberId, int id);

		/// <summary>
		/// 获取 项目字典 分类
		/// </summary>
		/// <param name="mainMemberId"></param>
		/// <param name="projectId"></param>
		/// <returns></returns>
		Task<List<string>> GetCategoryInProjectDict(int mainMemberId, int projectId);

		#endregion ProjectDict 项目字典

		#region 项目公式，用于复制公式

		/// <summary>
		/// 添加 项目字典
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddProjectFormula(Request<DbProjectFormula> request);

		/// <summary>
		/// 修改 项目字典
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditProjectFormula(Request<DbProjectFormula> request);

		/// <summary>
		/// 删除 项目字典
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteProjectFormula(Request<MemberIdDto> request);

		/// <summary>
		/// 选择所有替换代码
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> CheckedAllProjectFormula(Request<MemberIdDto> request);

		ValueTask<bool> UncheckedProjectFormulaByName(Request<GetProjectFormulaListDto> request);

		/// <summary>
		/// 获取所有 项目字典
		/// </summary>
		/// <returns></returns>
		Task<List<DbProjectFormula>> GetProjectFormulaAll(int mainMemberId, int projectId);

		/// <summary>
		/// 获取  项目字典 页
		/// </summary>
		/// <returns></returns>
		Task<Page<DbProjectFormula>> GetProjectFormulaPage(Request<GetProjectFormulaListDto> request);

		/// <summary>
		/// 获取 项目字典
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbProjectFormula> GetProjectFormulaById(int mainMemberId, int id);

		/// <summary>
		/// 获取 项目字典 分类
		/// </summary>
		/// <param name="mainMemberId"></param>
		/// <param name="projectId"></param>
		/// <returns></returns>
		Task<List<string>> GetCategoryInProjectFormula(int mainMemberId, int projectId);

		#endregion 项目公式，用于复制公式

		#region Factory 厂区

		/// <summary>
		/// 添加 厂区
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddFactory(Request<DbFactory> request);

		/// <summary>
		/// 修改 厂区
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditFactory(Request<DbFactory> request);

		/// <summary>
		/// 删除 厂区
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteFactory(Request<MemberIdDto> request);

		/// <summary>
		/// 获取所有 厂区
		/// </summary>
		/// <returns></returns>
		Task<List<DbFactory>> GetFactoryAll(int mainMemberId, int projectId);

		/// <summary>
		/// 获取  厂区 页
		/// </summary>
		/// <returns></returns>
		Task<Page<DbFactory>> GetFactoryPage(Request<GetFactoryListDto> request);

		/// <summary>
		/// 获取 厂区
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbFactory> GetFactoryById(int mainMemberId, int id);

		/// <summary>
		/// 获取 厂区 分类
		/// </summary>
		/// <param name="mainMemberId"></param>
		/// <param name="projectId"></param>
		/// <returns></returns>
		Task<List<string>> GetCategoryInFactory(int mainMemberId, int projectId);

		#endregion Factory 厂区

		#region FactoryMachine  厂区机械

		/// <summary>
		/// 添加 厂区机械
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddFactoryMachine(Request<DbFactoryMachine> request);

		/// <summary>
		/// 修改 厂区机械
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditFactoryMachine(Request<DbFactoryMachine> request);

		/// <summary>
		/// 删除 厂区机械
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteFactoryMachine(Request<MemberIdDto> request);

		/// <summary>
		/// 获取所有 厂区机械
		/// </summary>
		/// <returns></returns>
		Task<List<DbFactoryMachine>> GetFactoryMachineAll(int mainMemberId, int projectId);

		/// <summary>
		/// 获取 厂区机械 页
		/// </summary>
		/// <returns></returns>
		Task<Page<FactoryMachineDto>> GetFactoryMachinePage(Request<GetFactoryMachineListDto> request);

		/// <summary>
		/// 获取 厂区机械
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbFactoryMachine> GetFactoryMachineById(int mainMemberId, int id);

		/// <summary>
		/// 获取 厂区机械
		/// </summary>
		/// <param name="mainMemberId"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		Task<FactoryMachineDto> GetFactoryMachineByName(int mainMemberId, int projectId, string code);

		/// <summary>
		/// 获取 厂区机械 分类
		/// </summary>
		/// <param name="mainMemberId"></param>
		/// <param name="projectId"></param>
		/// <returns></returns>
		Task<List<string>> GetCategoryInFactoryMachine(int mainMemberId, int projectId);

		Task<List<string>> GetSubCategoryInFactoryMachine(int mainMemberId, int projectId, string category);

		#endregion FactoryMachine  厂区机械

		#region FactoryProcedure 厂区工艺

		/// <summary>
		/// 添加 厂区工艺
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddFactoryProcedure(Request<DbFactoryProcedure> request);

		/// <summary>
		/// 修改 厂区工艺
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditFactoryProcedure(Request<DbFactoryProcedure> request);

		/// <summary>
		/// 删除 厂区工艺
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteFactoryProcedure(Request<MemberIdDto> request);

		/// <summary>
		/// 获取所有 厂区工艺
		/// </summary>
		/// <returns></returns>
		Task<List<DbFactoryProcedure>> GetFactoryProcedureAll(int mainMemberId, int projectId, int? factoryId = null);

		/// <summary>
		/// 获取 厂区工艺 页
		/// </summary>
		/// <returns></returns>
		Task<Page<FactoryProcedureDto>> GetFactoryProcedurePage(Request<GetFactoryProcedureListDto> request);

		/// <summary>
		/// 获取 厂区工艺
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbFactoryProcedure> GetFactoryProcedureById(int mainMemberId, int id);

		/// <summary>
		/// 获取 厂区工艺
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbFactoryProcedure> GetFactoryProcedureByCode(int mainMemberId, int projectId, string code);

		/// <summary>
		/// 获取 厂区工艺 分类
		/// </summary>
		/// <param name="mainMemberId"></param>
		/// <param name="projectId"></param>
		/// <returns></returns>
		Task<List<string>> GetCategoryInFactoryProcedure(int mainMemberId, int projectId);

		#endregion FactoryProcedure 厂区工艺

		#region FactoryProcedureItem 厂区工艺 标记

		/// <summary>
		/// 添加 厂区工艺标记
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddFactoryProcedureItem(Request<DbFactoryProcedureItem> request);

		/// <summary>
		/// 修改 厂区工艺标记
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditFactoryProcedureItem(Request<DbFactoryProcedureItem> request);

		/// <summary>
		/// 删除 厂区工艺标记
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteFactoryProcedureItem(Request<MemberIdDto> request);

		/// <summary>
		/// 获取所有 厂区工艺标记
		/// </summary>
		/// <returns></returns>
		Task<List<FactoryProcedureItemDto>> GetFactoryProcedureItemAll(int mainMemberId, int projectId, int procedureId);

		/// <summary>
		/// 获取 厂区工艺标记 页
		/// </summary>
		/// <returns></returns>
		Task<Page<FactoryProcedureItemDto>> GetFactoryProcedureItemPage(Request<GetFactoryProcedureItemListDto> request);

		/// <summary>
		/// 获取 厂区工艺标记
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbFactoryProcedureItem> GetFactoryProcedureItemById(int mainMemberId, int id);

		#endregion FactoryProcedureItem 厂区工艺 标记

		#region App 流程信息

		/// <summary>
		/// 添加 流程信息
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddApp(Request<DbApp> request);

		/// <summary>
		/// 修改 流程信息
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditApp(Request<DbApp> request);

		/// <summary>
		/// 删除 流程信息
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteApp(Request<MemberIdDto> request);

		/// <summary>
		/// 获取所有 流程信息
		/// </summary>
		/// <returns></returns>
		Task<List<DbApp>> GetAppAll(int mainMemberId, int projectId);

		/// <summary>
		/// 获取 流程信息 页
		/// </summary>
		/// <returns></returns>
		Task<Page<DbApp>> GetAppPage(Request<GetAppListDto> request);

		/// <summary>
		/// 获取 流程信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbApp> GetAppById(int mainMemberId, int id);

		#endregion App 流程信息

		#region AppInput 输入项

		/// <summary>
		/// 添加 输入项
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddAppInput(Request<DbAppInput> request);

		/// <summary>
		/// 修改 输入项
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditAppInput(Request<DbAppInput> request);

		/// <summary>
		/// 删除 输入项
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteAppInput(Request<MemberIdDto> request);

		/// <summary>
		/// 获取所有 输入项
		/// </summary>
		/// <returns></returns>
		Task<List<DbAppInput>> GetAppInputAll(int mainMemberId, int projectId, int? appId = null);

		/// <summary>
		/// 获取 输入项 页
		/// </summary>
		/// <returns></returns>
		Task<Page<AppInputDto>> GetAppInputPage(Request<GetAppInputListDto> request);

		/// <summary>
		/// 获取 输入项
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbAppInput> GetAppInputById(int mainMemberId, int id);

		#endregion AppInput 输入项

		#region AppInitValue 初始值

		/// <summary>
		/// 添加 初始值
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddAppInitValue(Request<DbAppInitValue> request);

		/// <summary>
		/// 修改 初始值
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditAppInitValue(Request<DbAppInitValue> request);

		/// <summary>
		/// 删除 初始值
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteAppInitValue(Request<MemberIdDto> request);

		/// <summary>
		/// 获取所有 初始值
		/// </summary>
		/// <returns></returns>
		Task<List<DbAppInitValue>> GetAppInitValueAll(int mainMemberId, int projectId, int? appId = null);

		/// <summary>
		/// 获取 初始值 页
		/// </summary>
		/// <returns></returns>
		Task<Page<AppInitValueDto>> GetAppInitValuePage(Request<GetAppInitValueListDto> request);

		/// <summary>
		/// 获取 初始值
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbAppInitValue> GetAppInitValueById(int mainMemberId, int id);

		#endregion AppInitValue 初始值

		#region AppFlow 开始流程设计

		/// <summary>
		/// 添加 开始流程设计
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddAppFlow(Request<DbAppFlow> request);

		/// <summary>
		/// 修改 开始流程设计
		/// </summary>
		/// <param name="request"></param>
		ValueTask<bool> EditAppFlow(Request<DbAppFlow> request);

		/// <summary>
		/// 删除 开始流程设计
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteAppFlow(Request<MemberIdDto> request);

		/// <summary>
		/// 获取 开始流程设计
		/// </summary>
		/// <returns></returns>
		Task<List<DbAppFlow>> GetAppFlowAll(int mainMemberId, int projectId);

		/// <summary>
		/// 获取   开始流程设计 页
		/// </summary>
		/// <returns></returns>
		Task<Page<DbAppFlow>> GetAppFlowPage(Request<GetAppFlowListDto> request);

		/// <summary>
		/// 获取 开始流程设计
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbAppFlow> GetAppFlowByAppid(int mainMemberId, int appid);

		Task<DbAppFlow> GetAppFlowByAppCode(int mainMemberId, string appCode);

		#endregion AppFlow 开始流程设计

		#region ProjectInfo 项目文件

		ValueTask<bool> AddProjectFile(int mainMemberId, int projectId, int operatorId);

		ValueTask<bool> DeleteProjectFile(Request<MemberIdDto> request);

		Task<Page<ProjectFileDto>> GetProjectFilePage(Request<GetProjectLogListDto> request);

		Task<DbProjectFile> GetDefaultProjectFile(int mainMemberId, int projectId);

		ValueTask<bool> AddProjectWorkFile(int mainMemberId, int projectId, string json);

		Task<DbProjectWorkFile> GetProjectWorkFile(int mainMemberId);

		Task<DbProjectWorkFile> GetProjectWorkFile(int mainMemberId, string projectCode);

		#endregion ProjectInfo 项目文件

		#region GetAceKeywords

		Task<string> GetAceKeywords(int mainMemberId, int projectId, int appid = 0);

		#endregion GetAceKeywords
	}
}