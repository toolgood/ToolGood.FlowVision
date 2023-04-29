using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula.Functions;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using ToolGood.Algorithm2;
using ToolGood.FlowVision.Applications.Members.Impl;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowVision.Dtos.Projects.Dtos;
using ToolGood.ReadyGo3;

namespace ToolGood.FlowVision.Applications.Impl
{
    public class MemberFlowApplication : ApplicationBase, IMemberFlowApplication
    {
        private static Regex _inputNameRegex = new Regex("^[a-z\u3400-\u9fff_][0-9a-z\u3400-\u9fff_]*$", RegexOptions.IgnoreCase);
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MemberFlowApplication(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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

        /// <summary>
        /// 修改 项目
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> AddProject(Request<DbProject> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var db = DbProject.CreateBy(request);
            await helper.Insert_Async(db);
            await AddProjectOperationLog(db.Id, $"添加项目\"{db.Name}\"[{db.Id}]", request);

            // 增加默认厂区
            DbFactory factory = new DbFactory() {
                Code = "default",
                Comment = "默认",
                Category = "默认",
                SimplifyName = "默认",
                MainMemberId = db.MainMemberId,
                ProjectId = db.Id,
                CreateTime = DateTime.Now,
                CreateUserId = db.CreateUserId,
                ModifyTime = DateTime.Now,
                ModifyUserId = db.ModifyUserId,
            };
            await helper.Insert_Async(factory);
            return true;
        }

        /// <summary>
        /// 编辑 项目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async ValueTask<bool> EditProject(Request<DbProject> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbProject>(request.Data.Id);
            if (db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            if (request.MainMemberId == request.OperatorId) {
                await AddProjectOperationLog(db.Id, $"编辑项目\"{db.Name}\"[{db.Id}]", request);
            }
            return true;
        }

        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <returns></returns>
        public Task<List<DbProject>> GetProjectAll(int mainMemberId)
        {
            var read = GetReadHelper();
            return read.Select_Async<DbProject>("where MainMemberId=@0 and IsDelete=0", mainMemberId);
        }

        /// <summary>
        /// 获取 项目
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbProject> GetProjectById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbProject>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        /// <summary>
        /// 获取 项目 页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<Page<DbProject>> GetProjectPage(Request<GetProjectListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "desc";
            }

            var helper = GetReadHelper();
            return helper.Where<DbProject>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        #endregion Project 项目

        #region ProjectOperationLog 项目日志

        public async ValueTask<bool> AddProjectOperationLog(int projectId, string msg, IRequest request)
        {
            DbProjectLog operationLog = new DbProjectLog {
                ProjectId = projectId,
                Message = msg,

                CreateTime = DateTime.Now,
                Ip = request.Ip,
                UserAgent = request.UserAgent,
                SessionID = _httpContextAccessor.HttpContext.Session.Id,

                TrueName = request.OperatorTrueName,
                MemberId = request.OperatorId,
                MainMemberId = request.MainMemberId,
                Fingerprint = request.Fingerprint
            };

            var helper = GetWriteHelper();
            await helper.Insert_Async(operationLog);
            return true;
        }

        /// <summary>
        /// 获取 项目 页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<Page<ProjectLogDto>> GetProjectLogPage(Request<GetProjectLogListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "desc";
            }

            var helper = GetReadHelper();
            return helper.Where<ProjectLogDto>()
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .IfTrue(request.Data.ProjectId > 0).Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfTrue(request.Data.MemberId > 0).Where(q => q.MemberId == request.Data.MemberId)
                    .IfSet(request.Data.Name).Where(q => q.Message.Contains(request.Data.Name))
                    .IfNotNull(request.Data.StartTime).Where(q => q.CreateTime >= request.Data.StartTime)
                    .IfNotNull(request.Data.EndTime).Where(q => q.CreateTime < request.Data.EndTime.Value.AddDays(1))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 项目日志
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbProjectLog> GetProjectOperationLogById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbProjectLog>(q => q.Id == id && q.MainMemberId == mainMemberId).FirstOrDefault_Async();
        }

        #endregion ProjectOperationLog 项目日志

        #region ProjectDict 项目字典

        /// <summary>
        /// 添加 词汇
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddProjectDict(Request<DbProjectDict> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var db = DbProjectDict.CreateBy(request);
            await helper.Insert_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"添加词汇\"{db.Name}\"[{db.Id}]", request);
            return true;
        }

        /// <summary>
        /// 修改 词汇
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditProjectDict(Request<DbProjectDict> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbProjectDict>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"编辑词汇\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 删除 词汇
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteProjectDict(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbProjectDict>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            await AddProjectOperationLog(db.ProjectId, $"删除词汇\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 获取所有 词汇
        /// </summary>
        /// <returns></returns>
        public Task<List<DbProjectDict>> GetProjectDictAll(int mainMemberId, int projectId)
        {
            var read = GetReadHelper();
            return read.Select_Async<DbProjectDict>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
        }

        /// <summary>
        /// 获取  词汇 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<DbProjectDict>> GetProjectDictPage(Request<GetProjectDictListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetReadHelper();
            return helper.Where<DbProjectDict>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfSet(request.Data.Category).Where(q => q.Category == (request.Data.Category))
                    .IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 词汇
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbProjectDict> GetProjectDictById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbProjectDict>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        /// <summary>
        /// 获取 项目字典 分类
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<List<string>> GetCategoryInProjectDict(int mainMemberId, int projectId)
        {
            var helper = GetReadHelper();
            var sql = @"SELECT DISTINCT Category from flow_project_dict where IsDelete=0 and MainMemberId=@0 and ProjectId=@1 ORDER BY Category";
            return helper.Select_Async<string>(sql, mainMemberId, projectId);
        }

        #endregion ProjectDict 项目字典

        #region ProjectFormula 项目公式

        /// <summary>
        /// 添加 项目公式
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddProjectFormula(Request<DbProjectFormula> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (CheckInputName(request.Data.Name) == false) {
                request.Message = "出错了，公式名称有问题！"; return false;
            }

            var helper = GetWriteHelper();
            var count = helper.Count<DbProjectFormula>("where ProjectId=@0 and Name=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Name);
            if (count > 0) { request.Message = "出错了，公式名称重复！"; return false; }

            var db = DbProjectFormula.CreateBy(request);
            await helper.Insert_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"添加公式\"{db.Name}\"[{db.Id}]", request);
            return true;
        }

        /// <summary>
        /// 修改 项目公式
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditProjectFormula(Request<DbProjectFormula> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (CheckInputName(request.Data.Name) == false) {
                request.Message = "出错了，公式名称有问题！"; return false;
            }

            var helper = GetWriteHelper();
            var count = helper.Count<DbProjectFormula>("where Id<>@0 and ProjectId=@1 and Name=@2 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.Name);
            if (count > 0) { request.Message = "出错了，公式名称重复！"; return false; }
            var db = await helper.FirstOrDefault_Async<DbProjectFormula>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"编辑公式\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 删除 项目公式
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteProjectFormula(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbProjectFormula>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            await AddProjectOperationLog(db.ProjectId, $"删除公式\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 选择所有替换代码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> CheckedAllProjectFormula(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            if (request.Data.ProjectId == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var db = await helper.Update_Async<DbProjectFormula>("set IsReferenceCode=1 where ProjectId=@0 and MainMemberId=@1", request.Data.ProjectId.Value, request.MainMemberId);
            return db > 0;
        }

        public async ValueTask<bool> UncheckedProjectFormulaByName(Request<GetProjectFormulaListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var dbs = await helper.Select_Async<DbProjectFormula>("where ProjectId=@0 and MainMemberId=@1", request.Data.ProjectId, request.MainMemberId);
            if (dbs.Count == 0) { return true; }

            var names = request.Data.Name.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length == 0) { return true; }

            using (var tran = helper.UseTransaction()) {
                foreach (var db in dbs) {
                    var find = false;
                    if (db.Name.StartsWith("is") == false && db.Name.StartsWith("if") == false) {
                        foreach (var name in names) {
                            if (db.Name.Contains(name)) {
                                find = true;
                                break;
                            }
                        }
                    } else {
                        await helper.Update_Async<DbProjectFormula>("set IsReferenceCode=1 where id=@0", db.Id);
                    }
                    if (find) {
                        await helper.Update_Async<DbProjectFormula>("set IsReferenceCode=0 where id=@0", db.Id);
                    }
                }
                tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// 获取所有 项目公式
        /// </summary>
        /// <returns></returns>
        public Task<List<DbProjectFormula>> GetProjectFormulaAll(int mainMemberId, int projectId)
        {
            var read = GetReadHelper();
            return read.Select_Async<DbProjectFormula>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
        }

        /// <summary>
        /// 获取  项目公式 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<DbProjectFormula>> GetProjectFormulaPage(Request<GetProjectFormulaListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "IsReferenceCode asc,id asc";
                request.Data.Order = "";
            }

            var helper = GetReadHelper();
            return helper.Where<DbProjectFormula>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfSet(request.Data.Category).Where(q => q.Category == (request.Data.Category))
                    .IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 项目公式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbProjectFormula> GetProjectFormulaById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbProjectFormula>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        /// <summary>
        /// 获取 项目公式 分类
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<List<string>> GetCategoryInProjectFormula(int mainMemberId, int projectId)
        {
            var helper = GetReadHelper();
            var sql = @"SELECT DISTINCT Category from flow_project_formula where IsDelete=0 and MainMemberId=@0 and ProjectId=@1 ORDER BY Category";
            return helper.Select_Async<string>(sql, mainMemberId, projectId);
        }

        #endregion ProjectFormula 项目公式

        #region Factory 厂区

        /// <summary>
        /// 添加 厂区
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddFactory(Request<DbFactory> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var count = helper.Count<DbFactory>("where ProjectId=@0 and code=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Code);
            if (count > 0) { request.Message = "出错了，编码重复！"; return false; }
            count = helper.Count<DbFactory>("where ProjectId=@0 and SimplifyName=@1 and IsDelete=0", request.Data.ProjectId, request.Data.SimplifyName);
            if (count > 0) { request.Message = "出错了，厂区简称重复！"; return false; }

            var db = DbFactory.CreateBy(request);
            await helper.Insert_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"添加厂区\"{db.SimplifyName}\"[{db.Id}]", request);
            return true;
        }

        /// <summary>
        /// 修改 厂区
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditFactory(Request<DbFactory> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var count = helper.Count<DbFactory>("where Id<>@0 and ProjectId=@1 and code=@2 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.Code);
            if (count > 0) { request.Message = "出错了，编码重复！"; return false; }
            count = helper.Count<DbFactory>("where Id<>@0 and ProjectId=@1 and SimplifyName=@2 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.SimplifyName);
            if (count > 0) { request.Message = "出错了，厂区简称重复！"; return false; }

            var db = await helper.FirstOrDefault_Async<DbFactory>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"编辑厂区\"{db.SimplifyName}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 删除 厂区
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteFactory(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbFactory>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            await helper.Update_Async<DbFactoryProcedureItem>("set IsDelete=1 and DeleteUserId=@0 and DeleteTime=@1 where IsDelete=0 and FactoryId=@2", request.OperatorId, DateTime.Now, request.Data.Id);
            await helper.Update_Async<DbFactoryMachine>("set IsDelete=1 and DeleteUserId=@0 and DeleteTime=@1 where IsDelete=0 and FactoryId=@2", request.OperatorId, DateTime.Now, request.Data.Id);

            await AddProjectOperationLog(db.ProjectId, $"删除厂区\"{db.SimplifyName}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 获取所有 厂区
        /// </summary>
        /// <returns></returns>
        public Task<List<DbFactory>> GetFactoryAll(int mainMemberId, int projectId)
        {
            var read = GetReadHelper();
            return read.Select_Async<DbFactory>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
        }

        /// <summary>
        /// 获取  厂区 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<DbFactory>> GetFactoryPage(Request<GetFactoryListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "desc";
            }

            var helper = GetReadHelper();
            return helper.Where<DbFactory>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfSet(request.Data.Category).Where(q => q.Category == (request.Data.Category))
                    .IfSet(request.Data.Name).Where(q => q.SimplifyName.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 厂区
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbFactory> GetFactoryById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbFactory>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        /// <summary>
        /// 获取 厂区 分类
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<List<string>> GetCategoryInFactory(int mainMemberId, int projectId)
        {
            var helper = GetReadHelper();
            var sql = @"SELECT DISTINCT Category from flow_factory where IsDelete=0 and MainMemberId=@0 and ProjectId=@1 ORDER BY Category";
            return helper.Select_Async<string>(sql, mainMemberId, projectId);
        }

        #endregion Factory 厂区

        #region FactoryMachine  厂区机械

        /// <summary>
        /// 添加 厂区机械
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddFactoryMachine(Request<DbFactoryMachine> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var count = helper.Count<DbFactoryMachine>("where ProjectId=@0 and Name=@1 and FactoryId=@2 and IsDelete=0", request.Data.ProjectId, request.Data.Name, request.Data.FactoryId);
            if (count > 0) { request.Message = "出错了，机械名称重复！"; return false; }

            var db = DbFactoryMachine.CreateBy(request);
            using (var tran = helper.UseTransaction()) {
                await helper.Insert_Async(db);
                await AddProjectOperationLog(db.ProjectId, $"添加厂区机械\"{db.Name}\"[{db.Id}]", request);
                tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// 修改 厂区机械
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditFactoryMachine(Request<DbFactoryMachine> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();

            var count = helper.Count<DbFactoryMachine>("where Id<>@0 and ProjectId=@1 and Name=@2 and FactoryId=@3 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.Name, request.Data.FactoryId);
            if (count > 0) { request.Message = "出错了，机械名称重复！"; return false; }

            var db = await helper.FirstOrDefault_Async<DbFactoryMachine>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            using (var tran = helper.UseTransaction()) {
                await helper.Save_Async(db);
                await AddProjectOperationLog(db.ProjectId, $"编辑厂区机械\"{db.Name}\"[{db.Id}]", request);
                tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// 删除 厂区机械
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteFactoryMachine(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbFactoryMachine>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            await AddProjectOperationLog(db.ProjectId, $"删除厂区机械\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 获取所有 厂区机械
        /// </summary>
        /// <returns></returns>
        public Task<List<DbFactoryMachine>> GetFactoryMachineAll(int mainMemberId, int projectId)
        {
            var read = GetReadHelper();
            return read.Select_Async<DbFactoryMachine>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
        }

        /// <summary>
        /// 获取 厂区机械 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<FactoryMachineDto>> GetFactoryMachinePage(Request<GetFactoryMachineListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "Category asc, SubCategory asc, OrderNum asc,id asc";
                request.Data.Order = "";
            }

            var helper = GetReadHelper();
            return helper.Where<FactoryMachineDto>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfTrue(request.Data.Factory > 0).Where(q => q.FactoryId == request.Data.Factory)
                    .IfSet(request.Data.Category).Where(q => q.Category == (request.Data.Category))
                    .IfSet(request.Data.SubCategory).Where(q => q.SubCategory == (request.Data.SubCategory))
                    .IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 厂区机械
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbFactoryMachine> GetFactoryMachineById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbFactoryMachine>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        /// <summary>
        /// 获取 厂区机械
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<FactoryMachineDto> GetFactoryMachineByName(int mainMemberId, int projectId, string code)
        {
            return GetReadHelper().Where<FactoryMachineDto>(q => q.Name == code && q.ProjectId == projectId && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        /// <summary>
        /// 获取 厂区机械 分类
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<List<string>> GetCategoryInFactoryMachine(int mainMemberId, int projectId)
        {
            var helper = GetReadHelper();
            var sql = @"SELECT DISTINCT Category from flow_factory_machine where IsDelete=0 and MainMemberId=@0 and ProjectId=@1 ORDER BY Category asc";
            return helper.Select_Async<string>(sql, mainMemberId, projectId);
        }

        public Task<List<string>> GetSubCategoryInFactoryMachine(int mainMemberId, int projectId, string category)
        {
            var helper = GetReadHelper();
            var sql = @"SELECT DISTINCT SubCategory from flow_factory_machine where IsDelete=0 and MainMemberId=@0 and ProjectId=@1 and Category=@2 ORDER BY SubCategory asc";
            return helper.Select_Async<string>(sql, mainMemberId, projectId, category);
        }

        #endregion FactoryMachine  厂区机械

        #region FactoryProcedure 厂区工艺

        /// <summary>
        /// 添加 厂区工艺
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddFactoryProcedure(Request<DbFactoryProcedure> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var count = helper.Count<DbFactoryProcedure>("where ProjectId=@0 and code=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Code);
            if (count > 0) { request.Message = "出错了，编码重复！"; return false; }
            count = helper.Count<DbFactoryProcedure>("where ProjectId=@0 and Name=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Name);
            if (count > 0) { request.Message = "出错了，厂区工艺名称重复！"; return false; }

            var db = DbFactoryProcedure.CreateBy(request);
            using (var tran = helper.UseTransaction()) {
                await helper.Insert_Async(db);
                await AddProjectOperationLog(db.ProjectId, $"添加厂区工艺\"{db.Name}\"[{db.Id}]", request);
                // 增加机械标记
                var fids = db.FactoryIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var fid in fids) {
                    DbFactoryProcedureItem item = new DbFactoryProcedureItem() {
                        CreateTime = DateTime.Now,
                        CreateUserId = request.OperatorId,
                        ModifyTime = DateTime.Now,
                        ModifyUserId = request.OperatorId,
                        ProjectId = db.ProjectId,
                        MainMemberId = db.MainMemberId,
                        ProcedureId = db.Id,

                        FactoryId = int.Parse(fid),
                    };
                    helper.Insert(item);
                }
                tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// 修改 厂区工艺
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditFactoryProcedure(Request<DbFactoryProcedure> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var count = helper.Count<DbFactoryProcedure>("where Id<>@0 and ProjectId=@1 and code=@2 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.Code);
            if (count > 0) { request.Message = "出错了，编码重复！"; return false; }
            count = helper.Count<DbFactoryProcedure>("where Id<>@0 and ProjectId=@1 and Name=@2 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.Name);
            if (count > 0) { request.Message = "出错了，厂区工艺名称重复！"; return false; }

            var db = await helper.FirstOrDefault_Async<DbFactoryProcedure>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            var ids = await helper.Select_Async<int>("select FactoryId from flow_factory_procedure_item where ProjectId=@0 and ProcedureId=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Id);
            using (var tran = helper.UseTransaction()) {
                await helper.Save_Async(db);
                await AddProjectOperationLog(db.ProjectId, $"编辑厂区工艺\"{db.Name}\"[{db.Id}]", request);
                var fids = db.FactoryIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var fid in fids) {
                    if (ids.Contains(int.Parse(fid))) { continue; }
                    DbFactoryProcedureItem item = new DbFactoryProcedureItem() {
                        CreateTime = DateTime.Now,
                        CreateUserId = request.OperatorId,
                        ModifyTime = DateTime.Now,
                        ModifyUserId = request.OperatorId,
                        ProjectId = db.ProjectId,
                        MainMemberId = db.MainMemberId,
                        ProcedureId = db.Id,

                        FactoryId = int.Parse(fid),
                    };
                    helper.Insert(item);
                }
                tran.Complete();
            }

            return true;
        }

        /// <summary>
        /// 删除 厂区工艺
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteFactoryProcedure(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbFactoryProcedure>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            await helper.Update_Async<DbFactoryProcedureItem>("set IsDelete=1 and DeleteUserId=@0 and DeleteTime=@1 where IsDelete=0 and ProcedureId=@2", request.OperatorId, DateTime.Now, request.Data.Id);

            await AddProjectOperationLog(db.ProjectId, $"删除厂区工艺\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 获取所有 厂区工艺
        /// </summary>
        /// <returns></returns>
        public Task<List<DbFactoryProcedure>> GetFactoryProcedureAll(int mainMemberId, int projectId, int? factoryId)
        {
            var read = GetReadHelper();
            if (factoryId == null) {
                return read.Select_Async<DbFactoryProcedure>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
            }
            return read.Select_Async<DbFactoryProcedure>("where MainMemberId=@0 and projectId=@1 and factoryId=@2 and IsDelete=0", mainMemberId, projectId, factoryId);
        }

        /// <summary>
        /// 获取 厂区工艺 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<FactoryProcedureDto>> GetFactoryProcedurePage(Request<GetFactoryProcedureListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "desc";
            }

            var helper = GetReadHelper();
            return helper.Where<FactoryProcedureDto>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfTrue(request.Data.Factory > 0).Where(q => q.FactoryIds.Contains("," + request.Data.Factory + ","))
                    .IfSet(request.Data.Category).Where(q => q.Category == (request.Data.Category))
                    .IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 厂区工艺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbFactoryProcedure> GetFactoryProcedureById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbFactoryProcedure>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        /// <summary>
        /// 获取 厂区工艺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbFactoryProcedure> GetFactoryProcedureByCode(int mainMemberId, int projectId, string code)
        {
            return GetReadHelper().Where<DbFactoryProcedure>(q => q.Code == code && q.ProjectId == projectId && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        /// <summary>
        /// 获取 厂区工艺 分类
        /// </summary>
        /// <param name="mainMemberId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<List<string>> GetCategoryInFactoryProcedure(int mainMemberId, int projectId)
        {
            var helper = GetReadHelper();
            var sql = @"SELECT DISTINCT Category from flow_factory_procedure where IsDelete=0 and MainMemberId=@0 and ProjectId=@1 ORDER BY Category";
            return helper.Select_Async<string>(sql, mainMemberId, projectId);
        }

        #endregion FactoryProcedure 厂区工艺

        #region FactoryProcedureItem 厂区工艺 标记

        /// <summary>
        /// 添加 厂区工艺标记
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddFactoryProcedureItem(Request<DbFactoryProcedureItem> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var count = helper.Count<DbFactoryProcedureItem>("where ProjectId=@0 and code=@1 and FactoryId=@2 and IsDelete=0", request.Data.ProjectId, request.Data.Code, request.Data.FactoryId);
            if (count > 0) { request.Message = "出错了，编码重复！"; return false; }

            var db = DbFactoryProcedureItem.CreateBy(request);
            await helper.Insert_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"添加厂区工艺标记\"{db.Name}\"[{db.Id}]", request);

            var ids = await helper.Select_Async<int>("select FactoryId from flow_factory_procedure_item where ProjectId=@0 and ProcedureId=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Id);
            ids = ids.Distinct().ToList();
            helper.Update<DbFactoryProcedure>("set FactoryIds=@0 where id=@1", ',' + string.Join(',', ids) + ',', db.ProcedureId);

            return true;
        }

        /// <summary>
        /// 修改 厂区工艺标记
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditFactoryProcedureItem(Request<DbFactoryProcedureItem> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var count = helper.Count<DbFactoryProcedureItem>("where Id<>@0 and ProjectId=@1 and code=@2 and FactoryId=@3 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.Code, request.Data.FactoryId);
            if (count > 0) { request.Message = "出错了，编码重复！"; return false; }

            var db = await helper.FirstOrDefault_Async<DbFactoryProcedureItem>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"编辑厂区工艺标记\"{db.Name}\"[{db.Id}]", request);

            var ids = await helper.Select_Async<int>("select FactoryId from flow_factory_procedure_item where ProjectId=@0 and ProcedureId=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Id);
            ids = ids.Distinct().ToList();
            helper.Update<DbFactoryProcedure>("set FactoryIds=@0 where id=@1", ',' + string.Join(',', ids) + ',', db.ProcedureId);

            return true;
        }

        /// <summary>
        /// 删除 厂区工艺标记
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteFactoryProcedureItem(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbFactoryProcedureItem>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            await AddProjectOperationLog(db.ProjectId, $"删除厂区工艺标记\"{db.Name}\"[{db.Id}]", request);

            var ids = await helper.Select_Async<int>("select FactoryId from flow_factory_procedure_item where ProjectId=@0 and ProcedureId=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Id);
            ids = ids.Distinct().ToList();
            await helper.Update_Async<DbFactoryProcedure>("set FactoryIds=@0 where id=@1", ',' + string.Join(',', ids) + ',', db.ProcedureId);

            return true;
        }

        /// <summary>
        /// 获取所有 厂区工艺标记
        /// </summary>
        /// <returns></returns>
        public Task<List<FactoryProcedureItemDto>> GetFactoryProcedureItemAll(int mainMemberId, int projectId, int procedureId)
        {
            var read = GetReadHelper();
            return read.Select_Async<FactoryProcedureItemDto>("where MainMemberId=@0 and projectId=@1 and procedureId=@2 and IsDelete=0", mainMemberId, projectId, procedureId);
        }

        /// <summary>
        /// 获取 厂区工艺标记 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<FactoryProcedureItemDto>> GetFactoryProcedureItemPage(Request<GetFactoryProcedureItemListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "asc";
            }

            var helper = GetReadHelper();
            return helper.Where<FactoryProcedureItemDto>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .Where(q => q.ProcedureId == request.Data.ProcedureId)
                    .IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 厂区工艺标记
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbFactoryProcedureItem> GetFactoryProcedureItemById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbFactoryProcedureItem>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        #endregion FactoryProcedureItem 厂区工艺 标记

        #region App 流程信息

        /// <summary>
        /// 添加 流程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddApp(Request<DbApp> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var count = helper.Count<DbApp>("where ProjectId=@0 and code=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Code);
            if (count > 0) { request.Message = "出错了，编码重复！"; return false; }

            count = helper.Count<DbApp>("where ProjectId=@0 and Name=@1 and IsDelete=0", request.Data.ProjectId, request.Data.Name);
            if (count > 0) { request.Message = "出错了，流程名称重复！"; return false; }

            var db = DbApp.CreateBy(request);
            await helper.Insert_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"添加流程信息\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 修改 流程信息
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditApp(Request<DbApp> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbApp>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            var count = helper.Count<DbApp>("where Id<>@0 and ProjectId=@1 and code=@2 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.Code);
            if (count > 0) { request.Message = "出错了，编码重复！"; return false; }
            count = helper.Count<DbApp>("where Id<>@0 and ProjectId=@1 and Name=@2 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.Name);
            if (count > 0) { request.Message = "出错了，流程名称重复！"; return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"编辑流程信息\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 删除 流程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteApp(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbApp>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            //await helper.Update_Async<DbAppFlow>("set IsDelete=1 and DeleteUserId=@0 and DeleteTime=@1 where IsDelete=0 and AppId=@2", request.OperatorId, DateTime.Now, request.Data.Id);
            //await helper.Update_Async<DbAppInitValue>("set IsDelete=1 and DeleteUserId=@0 and DeleteTime=@1 where IsDelete=0 and AppId=@2", request.OperatorId, DateTime.Now, request.Data.Id);
            //await helper.Update_Async<DbAppInput>("set IsDelete=1 and DeleteUserId=@0 and DeleteTime=@1 where IsDelete=0 and AppId=@2", request.OperatorId, DateTime.Now, request.Data.Id);

            await AddProjectOperationLog(db.ProjectId, $"删除流程信息\"{db.Name}\"[{db.Id}]", request);
            return true;
        }

        /// <summary>
        /// 获取所有 流程信息
        /// </summary>
        /// <returns></returns>
        public Task<List<DbApp>> GetAppAll(int mainMemberId, int projectId)
        {
            var read = GetReadHelper();
            return read.Select_Async<DbApp>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
        }

        /// <summary>
        /// 获取 流程信息 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<DbApp>> GetAppPage(Request<GetAppListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "desc";
            }

            var helper = GetReadHelper();
            return helper.Where<DbApp>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 流程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbApp> GetAppById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbApp>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        #endregion App 流程信息

        #region AppInput 输入项

        /// <summary>
        /// 添加 输入项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddAppInput(Request<DbAppInput> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            if (request.Data.IsRequired && request.Data.UseDefaultValue) { request.Message = "出错了，输入项不能是 [必填] + [使用默认值]！"; return false; }

            if (CheckInputName(request.Data.InputName) == false) {
                request.Message = "出错了，输入名称有问题！"; return false;
            }

            var helper = GetWriteHelper();
            //var count = helper.Count<DbAppInput>("where ProjectId=@0 and AppId=@1 and InputName=@2 and IsDelete=0", request.Data.ProjectId, request.Data.AppId, request.Data.InputName);
            //if (count > 0) { request.Message = "出错了，输入名称重复！"; return false; }

            var db = DbAppInput.CreateBy(request);
            await helper.Insert_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"添加输入项\"{db.InputName}\"[{db.Id}]", request);
            return true;
        }

        /// <summary>
        /// 修改 输入项
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditAppInput(Request<DbAppInput> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            if (request.Data.IsRequired && request.Data.UseDefaultValue) { request.Message = "出错了，输入项不能是 [必填] + [使用默认值]！"; return false; }

            if (CheckInputName(request.Data.InputName) == false) {
                request.Message = "出错了，输入名称有问题！"; return false;
            }

            var helper = GetWriteHelper();
            //var count = helper.Count<DbAppInput>("where Id<>@0 and ProjectId=@1 and AppId=@2 and InputName=@3 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.AppId, request.Data.InputName);
            //if (count > 0) { request.Message = "出错了，输入名称重复！"; return false; }

            var db = await helper.FirstOrDefault_Async<DbAppInput>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"编辑输入项\"{db.InputName}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 删除 输入项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteAppInput(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbAppInput>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            await AddProjectOperationLog(db.ProjectId, $"删除输入项\"{db.InputName}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 获取所有 输入项
        /// </summary>
        /// <returns></returns>
        public Task<List<DbAppInput>> GetAppInputAll(int mainMemberId, int projectId, int? appId)
        {
            var read = GetReadHelper();
            if (appId == null) {
                return read.Select_Async<DbAppInput>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
            }
            return read.Select_Async<DbAppInput>("where MainMemberId=@0 and projectId=@1 and factoryId=@2 and IsDelete=0", mainMemberId, projectId, appId);
        }

        /// <summary>
        /// 获取 输入项 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<AppInputDto>> GetAppInputPage(Request<GetAppInputListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "desc";
            }

            var helper = GetReadHelper();
            return helper.Where<AppInputDto>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfTrue(request.Data.AppId > 0).Where(q => q.AppIds.Contains("," + request.Data.AppId + ","))
                    .IfSet(request.Data.Name).Where(q => q.InputName.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 输入项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbAppInput> GetAppInputById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbAppInput>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        #endregion AppInput 输入项

        #region AppInitValue 初始值

        /// <summary>
        /// 添加 初始值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddAppInitValue(Request<DbAppInitValue> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (CheckInputName(request.Data.Name) == false) {
                request.Message = "出错了，初始名称有问题！"; return false;
            }

            var helper = GetWriteHelper();
            //var count = helper.Count<DbAppInitValue>("where ProjectId=@0 and AppId=@1 and Name=@2 and IsDelete=0", request.Data.ProjectId, request.Data.AppId, request.Data.Name);
            //if (count > 0) { request.Message = "出错了，初始名称重复！"; return false; }

            var db = DbAppInitValue.CreateBy(request);
            await helper.Insert_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"添加初始值\"{db.Name}\"[{db.Id}]", request);
            return true;
        }

        /// <summary>
        /// 修改 初始值
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditAppInitValue(Request<DbAppInitValue> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (CheckInputName(request.Data.Name) == false) {
                request.Message = "出错了，初始名称有问题！"; return false;
            }

            var helper = GetWriteHelper();
            //var count = helper.Count<DbAppInitValue>("where Id<>@0 and ProjectId=@1 and AppId=@2 and Name=@3 and IsDelete=0", request.Data.Id, request.Data.ProjectId, request.Data.AppId, request.Data.Name);
            //if (count > 0) { request.Message = "出错了，初始名称重复！"; return false; }

            var db = await helper.FirstOrDefault_Async<DbAppInitValue>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"编辑初始值\"{db.Name}\"[{db.Id}]", request);
            return true;
        }

        /// <summary>
        /// 删除 初始值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteAppInitValue(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbAppInitValue>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);

            await AddProjectOperationLog(db.ProjectId, $"删除初始值\"{db.Name}\"[{db.Id}]", request);

            return true;
        }

        /// <summary>
        /// 获取所有 初始值
        /// </summary>
        /// <returns></returns>
        public Task<List<DbAppInitValue>> GetAppInitValueAll(int mainMemberId, int projectId, int? appId)
        {
            var read = GetReadHelper();
            if (appId == null) {
                return read.Select_Async<DbAppInitValue>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
            }
            return read.Select_Async<DbAppInitValue>("where MainMemberId=@0 and projectId=@1 and factoryId=@2 and IsDelete=0", mainMemberId, projectId, appId);
        }

        /// <summary>
        /// 获取 初始值 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<AppInitValueDto>> GetAppInitValuePage(Request<GetAppInitValueListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "desc";
            }

            var helper = GetReadHelper();
            return helper.Where<AppInitValueDto>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfTrue(request.Data.AppId > 0).Where(q => q.AppIds.Contains("," + request.Data.AppId + ","))
                    .IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 初始值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DbAppInitValue> GetAppInitValueById(int mainMemberId, int id)
        {
            return GetReadHelper().Where<DbAppInitValue>(q => q.Id == id && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        #endregion AppInitValue 初始值

        #region AppFlow 开始流程设计

        /// <summary>
        /// 添加 开始流程设计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddAppFlow(Request<DbAppFlow> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetWriteHelper();
            var db = DbAppFlow.CreateBy(request);
            await helper.Insert_Async(db);
            var appInfo = await helper.FirstOrDefault_Async<DbApp>("where isdelete=0 and MainMemberId=@0 and ProjectId=@1  and code=@2", db.MainMemberId, db.ProjectId, db.AppCode);
            await AddProjectOperationLog(db.ProjectId, $"添加流程设计\"{appInfo.Name}\"[{db.Id}]", request);

            //try {
            //    var log = DbAppFlowLog.CreateBy(request);
            //    var logHelper = Config.LogHelper;
            //    await logHelper.Insert_Async(log);
            //} catch (Exception) { }
            return true;
        }

        /// <summary>
        /// 修改 开始流程设计
        /// </summary>
        /// <param name="request"></param>
        public async ValueTask<bool> EditAppFlow(Request<DbAppFlow> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbAppFlow>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.EditBy(request);
            await helper.Save_Async(db);
            var appInfo = await helper.FirstOrDefault_Async<DbApp>("where isdelete=0 and MainMemberId=@0 and ProjectId=@1  and code=@2", db.MainMemberId, db.ProjectId, db.AppCode);
            await AddProjectOperationLog(db.ProjectId, $"编辑流程设计\"{appInfo.Name}\"[{db.Id}]", request);

            //try {
            //    var logHelper = Config.LogHelper;
            //    var db2 = await logHelper.FirstOrDefault_Async<DbAppFlowLog>("where MainMemberId=@0 and ProjectId=@1 and AppCode=@2 order by id desc limit 1", db.MainMemberId, db.ProjectId, db.AppCode);
            //    if (db2.FlowString!= db.FlowString) {
            //        var log = DbAppFlowLog.CreateBy(request);
            //        await logHelper.Insert_Async(log);
            //    }
            //} catch (Exception) { }
            return true;
        }

        /// <summary>
        /// 删除 开始流程设计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteAppFlow(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbAppFlow>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            db.IsDelete = true;
            db.DeleteUserId = request.OperatorId;
            db.DeleteTime = DateTime.Now;
            await helper.Save_Async(db);
            var appInfo = await helper.FirstOrDefault_Async<DbApp>("where isdelete=0 and MainMemberId=@0 and ProjectId=@1  and code=@2", db.MainMemberId, db.ProjectId, db.AppCode);
            await AddProjectOperationLog(db.ProjectId, $"删除流程设计\"{appInfo.Name}\"[{db.Id}]", request);
            return true;
        }

        /// <summary>
        /// 获取 开始流程设计
        /// </summary>
        /// <returns></returns>
        public Task<List<DbAppFlow>> GetAppFlowAll(int mainMemberId, int projectId)
        {
            var read = GetReadHelper();
            return read.Select_Async<DbAppFlow>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
        }

        /// <summary>
        /// 获取   开始流程设计 页
        /// </summary>
        /// <returns></returns>
        public Task<Page<DbAppFlow>> GetAppFlowPage(Request<GetAppFlowListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            var helper = GetReadHelper();
            return helper.Where<DbAppFlow>(q => q.IsDelete == false)
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .Where(q => q.ProjectId == request.Data.ProjectId)
                    //.IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        /// <summary>
        /// 获取 开始流程设计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DbAppFlow> GetAppFlowByAppid(int mainMemberId, int appid)
        {
            var helper = GetReadHelper();
            var app = await helper.FirstOrDefault_Async<DbApp>("where isDelete=0 and MainMemberId=@0 and id=@1", mainMemberId, appid);
            if (app != null) {
                return await helper.Where<DbAppFlow>(q => q.AppCode == app.Code && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
            }
            return null;
        }

        public Task<DbAppFlow> GetAppFlowByAppCode(int mainMemberId, string appCode)
        {
            var helper = GetReadHelper();
            return helper.Where<DbAppFlow>(q => q.AppCode == appCode && q.MainMemberId == mainMemberId && q.IsDelete == false).FirstOrDefault_Async();
        }

        #endregion AppFlow 开始流程设计

        #region ProjectInfo 项目文件

        public async ValueTask<bool> AddProjectFile(int mainMemberId, int projectId, int operatorId)
        {
            DbProjectFile db = new DbProjectFile() {
                MainMemberId = mainMemberId,
                ProjectId = projectId,
                CreateTime = DateTime.Now,
                CreateUserId = operatorId
            };

            var helper = GetWriteHelper();
            await helper.Insert_Async(db);
            return true;
        }

        public async ValueTask<bool> DeleteProjectFile(Request<MemberIdDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }
            var helper = GetWriteHelper();
            var db = await helper.FirstOrDefault_Async<DbProjectFile>(request.Data.Id);
            if (db.ProjectId != request.Data.ProjectId || db.MainMemberId != request.MainMemberId) { return false; }

            await helper.Delete_Async(db);
            await AddProjectOperationLog(db.ProjectId, $"删除项目文件[{db.Id}]", request);
            return true;
        }

        public Task<Page<ProjectFileDto>> GetProjectFilePage(Request<GetProjectLogListDto> request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Data == null) { throw new ArgumentException("Data is null"); }

            if (string.IsNullOrEmpty(request.Data.Field)) {
                request.Data.Field = "id";
                request.Data.Order = "desc";
            }

            var helper = GetReadHelper();
            return helper.Where<ProjectFileDto>()
                    .Where(q => q.MainMemberId == request.MainMemberId)
                    .IfTrue(request.Data.ProjectId > 0).Where(q => q.ProjectId == request.Data.ProjectId)
                    .IfNotNull(request.Data.StartTime).Where(q => q.CreateTime >= request.Data.StartTime)
                    .IfNotNull(request.Data.EndTime).Where(q => q.CreateTime < request.Data.EndTime.Value.AddDays(1))
                    .IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
                    .Page_Async(request.Data.PageIndex, request.Data.PageSize);
        }

        public Task<DbProjectFile> GetDefaultProjectFile(int mainMemberId, int projectId)
        {
            var helper = GetReadHelper();
            return helper.FirstOrDefault_Async<DbProjectFile>("where MainMemberId=@0 and ProjectId=@1 order by id desc limit 1", mainMemberId, projectId);
        }

        public async ValueTask<bool> AddProjectWorkFile(int mainMemberId, int projectId, string json)
        {
            var helper = GetWriteHelper();
            var project = await helper.FirstOrDefault_Async<DbProject>("where MainMemberId=@0 and Id=@1", mainMemberId, projectId);
            if (project == null) { return false; }

            var projectWorkFile = await helper.FirstOrDefault_Async<DbProjectWorkFile>("where MainMemberId=@0 and ProjectCode=@1", mainMemberId, project.Code);
            if (projectWorkFile == null) {
                DbProjectWorkFile workFile = new DbProjectWorkFile() {
                    CreateTime = DateTime.Now,
                    Json = json,
                    ProjectCode = project.Code,
                    MainMemberId = mainMemberId,
                };
                await helper.Insert_Async(workFile);
            } else {
                projectWorkFile.Json = json;
                await helper.Update_Async(projectWorkFile);
            }
            return true;
        }

        public async Task<DbProjectWorkFile> GetProjectWorkFile(int mainMemberId)
        {
            var helper = GetReadHelper();
            var project = await helper.FirstOrDefault_Async<DbProject>("where MainMemberId=@0", mainMemberId);
            if (project == null) { return null; }
            return await helper.FirstOrDefault_Async<DbProjectWorkFile>("where MainMemberId=@0 and ProjectCode=@1", mainMemberId, project.Code);
        }

        public Task<DbProjectWorkFile> GetProjectWorkFile(int mainMemberId, string projectCode)
        {
            var helper = GetReadHelper();
            return helper.FirstOrDefault_Async<DbProjectWorkFile>("where MainMemberId=@0 and ProjectCode=@1", mainMemberId, projectCode);
        }

        #endregion ProjectInfo 项目文件

        #region GetAceKeywords

        public async Task<string> GetAceKeywords(int mainMemberId, int projectId, int appid = 0)
        {
            List<KeyValue> temp = new List<KeyValue>();

            #region 基础方法

            temp.Add(new KeyValue("if(条件, 真值, 可选假值)", "判断真假"));
            temp.Add(new KeyValue("ifError(测试条件, 真值, 可选假值)", "判断是否出错"));
            temp.Add(new KeyValue("isError(测试条件, 可选替换值)", "判断是否出错"));
            temp.Add(new KeyValue("isNull(值, 可选替换值)", "判断是否为空"));
            temp.Add(new KeyValue("isNullOrError(值, 可选替换值)", "判断是否空或错误"));
            temp.Add(new KeyValue("isNumber(值)", "判断是否数值"));
            temp.Add(new KeyValue("isText(值)", "判断是否文字"));
            temp.Add(new KeyValue("isNonText(值)", "判断是否非文字"));
            temp.Add(new KeyValue("isLogical(值)", "判断是否逻辑值"));
            temp.Add(new KeyValue("isEven(值)", "判断是否偶数"));
            temp.Add(new KeyValue("isOdd(值)", "判断是否奇数"));
            temp.Add(new KeyValue("and(逻辑值A, 逻辑值B,,,)", "并操作"));
            temp.Add(new KeyValue("or(逻辑值A, 逻辑值B,,,)", "或操作"));
            temp.Add(new KeyValue("not(逻辑值)", "求反"));
            temp.Add(new KeyValue("true", "逻辑值TRUE"));
            temp.Add(new KeyValue("false", "逻辑值FALSE"));
            temp.Add(new KeyValue("e", "常数e"));
            temp.Add(new KeyValue("pi", "常数pi"));
            temp.Add(new KeyValue("abs(数值)", "绝对值"));
            temp.Add(new KeyValue("quotient(除数, 被除数)", "商的整数部分"));
            temp.Add(new KeyValue("mod(除数, 被除数)", "余数"));
            temp.Add(new KeyValue("sign(数值)", "数值的符号"));
            temp.Add(new KeyValue("sqrt(数值)", "正平方根"));
            temp.Add(new KeyValue("trunc(数值)", "数值截尾取整"));
            temp.Add(new KeyValue("int(数值)", "取整数"));
            temp.Add(new KeyValue("gcd(数值A, 数值B,,,)", "最大公约数"));
            temp.Add(new KeyValue("lcm(数值A, 数值B,,,)", "最小公倍数"));
            temp.Add(new KeyValue("combin(总数, 排列数)", "计算集合组合数"));
            temp.Add(new KeyValue("permut(总数, 排列数)", "计算集合排列数"));
            temp.Add(new KeyValue("fixed(数值, 小数位数, 有无逗号分隔符)", "数值转文本带小数"));
            temp.Add(new KeyValue("degrees(弧度)", "弧度转度"));
            temp.Add(new KeyValue("radians(度)", "度转弧度"));
            temp.Add(new KeyValue("cos(弧度)", "余弦值"));
            temp.Add(new KeyValue("cosh(弧度)", "双曲余弦值"));
            temp.Add(new KeyValue("sin(弧度)", "正弦值"));
            temp.Add(new KeyValue("sinh(弧度)", "双曲正弦值"));
            temp.Add(new KeyValue("tan(弧度)", "正切值"));
            temp.Add(new KeyValue("tanh(弧度)", "双曲正切值"));
            temp.Add(new KeyValue("acos(数值)", "反余弦值"));
            temp.Add(new KeyValue("acosh(数值)", "反双曲余弦值"));
            temp.Add(new KeyValue("asin(数值)", "反正弦值"));
            temp.Add(new KeyValue("asinh(数值)", "反双曲正弦值"));
            temp.Add(new KeyValue("atan(数值)", "反正切值"));
            temp.Add(new KeyValue("atanh(数值)", "反双曲正切值"));
            temp.Add(new KeyValue("atan2(数值)", "反正切"));
            temp.Add(new KeyValue("round(数值, 可选小数位数)", "四舍五入"));
            temp.Add(new KeyValue("roundDown(数值, 小数位数)", "向下舍入数值"));
            temp.Add(new KeyValue("roundUp(数值, 小数位数)", "向上舍入数值"));
            temp.Add(new KeyValue("ceiling(数值, 舍入基数)", "向上舍入"));
            temp.Add(new KeyValue("floor(数值, 舍入基数)", "向下舍入"));
            temp.Add(new KeyValue("even(数值)", "向上取偶数"));
            temp.Add(new KeyValue("odd(数值)", "向上取奇数"));
            temp.Add(new KeyValue("mRound(数值, 舍入基数)", "四舍五入"));
            temp.Add(new KeyValue("rand()", "0到1的随机数"));
            temp.Add(new KeyValue("randBetween(最小整数, 最大整数)", "随机整数"));
            temp.Add(new KeyValue("fact(数值)", "阶乘"));
            temp.Add(new KeyValue("factdouble(数值)", "双倍阶乘"));
            temp.Add(new KeyValue("power(数值, 幂)", "数的乘幂"));
            temp.Add(new KeyValue("exp(幂)", "e的指定数乘幂"));
            temp.Add(new KeyValue("ln(数值)", "自然对数"));
            temp.Add(new KeyValue("log(数值, 可选底数)", "对数"));
            temp.Add(new KeyValue("log10(数值)", "10的对数"));
            temp.Add(new KeyValue("multiNomial(数值A, 数值B,,,)", "多项式分布"));
            temp.Add(new KeyValue("product(数值A, 数值B,,,)", "所有数值相乘,乘积值"));
            temp.Add(new KeyValue("sqrtPi(数值)", "数与PI的乘积的平方根"));
            temp.Add(new KeyValue("sumQq(数值A, 数值B,,,)", "平方和"));
            temp.Add(new KeyValue("asc(字符串)", "转半角"));
            temp.Add(new KeyValue("jis(字符串)", "转全角"));
            temp.Add(new KeyValue("wideChar(字符串)", "转全角"));
            temp.Add(new KeyValue("char(字符串)", "数字代码转字符"));
            temp.Add(new KeyValue("clean(字符串)", "删除不可见字符"));
            temp.Add(new KeyValue("code(字符串)", "字符转数值代码"));
            temp.Add(new KeyValue("concatenate(字符串A, 字符串B,,,)", "合并到文本"));
            temp.Add(new KeyValue("exact(字符串A, 字符串B)", "判断文本是否相同"));
            temp.Add(new KeyValue("find(要查找的字符串, 被查找的字符串, 可选开始位置)", "查找文本"));
            temp.Add(new KeyValue("left(字符串, 可选字符个数)", "截取左边的字符"));
            temp.Add(new KeyValue("len(字符串)", "字符个数"));
            temp.Add(new KeyValue("mid(字符串, 开始位置, 可选字符个数)", "截取文本"));
            temp.Add(new KeyValue("proper(字符串)", "每个单词首字母大写"));
            temp.Add(new KeyValue("replace(原字符串, 开始位置, 字符个数, 新字符串)", "替换字符"));
            temp.Add(new KeyValue("Replace(原字符串, 要替换的字符串, 新字符串)", "替换字符"));
            temp.Add(new KeyValue("rept(字符串, 重复次数)", "重复文本"));
            temp.Add(new KeyValue("right(字符串, 可选字符个数)", "截取右边的字符"));
            temp.Add(new KeyValue("rmb(数值)", "转大写数值文本"));
            temp.Add(new KeyValue("search(要找的字符串, 被查找的字符串, 可选开始位置)", "查找文本位置"));
            temp.Add(new KeyValue("substitute(字符串, 原字符串, 新字符串, 可选替换序号)", "替换字符"));
            temp.Add(new KeyValue("t(数值)", "转文本"));
            temp.Add(new KeyValue("text(数值, 数值格式)", "数值转换为文本"));
            temp.Add(new KeyValue("trim(字符串)", "删除文本中的空格"));
            temp.Add(new KeyValue("lower(字符串)", "英文转小写"));
            temp.Add(new KeyValue("tolower(字符串)", "英文转小写"));
            temp.Add(new KeyValue("upper(字符串)", "英文转大写"));
            temp.Add(new KeyValue("toupper(字符串)", "英文转大写"));
            temp.Add(new KeyValue("value(字符串)", "文本转数值"));
            temp.Add(new KeyValue("now()", "当前日期和时间"));
            temp.Add(new KeyValue("today()", "今日"));
            temp.Add(new KeyValue("dateValue(字符串)", "文本转日期"));
            temp.Add(new KeyValue("timeValue(字符串)", "文本转时间"));
            temp.Add(new KeyValue("date(年, 月, 日, 可选时, 可选分, 可选秒)", "日期"));
            temp.Add(new KeyValue("time(时, 分, 秒)", "时间"));
            temp.Add(new KeyValue("year(日期)", "年"));
            temp.Add(new KeyValue("month(日期)", "月"));
            temp.Add(new KeyValue("day(日期)", "日"));
            temp.Add(new KeyValue("hour(日期)", "小时"));
            temp.Add(new KeyValue("minute(日期)", "分钟"));
            temp.Add(new KeyValue("second(日期)", "秒"));
            temp.Add(new KeyValue("weekday(日期)", "星期几"));
            temp.Add(new KeyValue("dateDif(开始日期, 结束日期, 类型Y/M/D/YD/MD/YM)", "相隔天数"));
            temp.Add(new KeyValue("days360(开始日期, 结束日期, 可选选项0/1)", "相隔天数"));
            temp.Add(new KeyValue("eDate(开始日期, 月数)", "计算以后日期"));
            temp.Add(new KeyValue("eoMonth(开始日期, 月数)", "计算以后月份最后一天"));
            temp.Add(new KeyValue("netWorkdays(开始日期, 结束日期, 可选假日)", "计算工作天数"));
            temp.Add(new KeyValue("workDay(开始日期, 天数, 可选假日)", "计算以后工作日期"));
            temp.Add(new KeyValue("weekNum(日期, 类型1/2)", "求周数"));
            temp.Add(new KeyValue("max(数值A, 数值B,,,)", "最大值"));
            temp.Add(new KeyValue("median(数值A, 数值B,,,)", "中值"));
            temp.Add(new KeyValue("min(数值A, 数值B,,,)", "最小值"));
            temp.Add(new KeyValue("quartile(数组, 四分位0-4)", "集合四分位数"));
            temp.Add(new KeyValue("mode(数值A, 数值B,,,)", "频率最高的数值"));
            temp.Add(new KeyValue("large(数组, 序列K)", "第k个最大值"));
            temp.Add(new KeyValue("small(数组, 序列K)", "第k个最小值"));
            temp.Add(new KeyValue("percentile(数组, 序列K)", "第k个百分位值"));
            temp.Add(new KeyValue("percentRank(数组, 序列K)", "中值的百分比排位"));
            temp.Add(new KeyValue("average(数值A, 数值B,,,)", "平均值"));
            temp.Add(new KeyValue("averageIf(数组, 条件, 可选数组)", "平均值"));
            temp.Add(new KeyValue("geoMean(数值A, 数值B,,,)", "几何平均值"));
            temp.Add(new KeyValue("harMean(数值A, 数值B,,,)", "调和平均值"));
            temp.Add(new KeyValue("count(数值A, 数值B,,,)", "集合个数"));
            temp.Add(new KeyValue("countIf(数组, 条件, 可选数组)", "集合个数"));
            temp.Add(new KeyValue("sum(数值A, 数值B,,,)", "求和"));
            temp.Add(new KeyValue("sumIf(数组, 条件, 可选数组)", "求和"));
            temp.Add(new KeyValue("aveDev(数值A, 数值B,,,)", "平均差"));
            temp.Add(new KeyValue("stDev(数值A, 数值B,,,)", "样本估算标准偏差"));
            temp.Add(new KeyValue("stDevp(数值A, 数值B,,,)", "样本总体标准偏差"));
            temp.Add(new KeyValue("devSq(数值A, 数值B,,,)", "偏差的平方和"));
            temp.Add(new KeyValue("var(数值A, 数值B,,,)", "样本估算方差"));
            temp.Add(new KeyValue("varp(数值A, 数值B,,,)", "样本总体计算方差"));
            temp.Add(new KeyValue("normDist(数值, 算术平均值, 标准偏差, 返回类型0/1)", "正态累积分布"));
            temp.Add(new KeyValue("normInv(分布概率, 算术平均值, 标准偏差)", "反正态累积分布"));
            temp.Add(new KeyValue("normSDist(数值)", "标准正态累积分布"));
            temp.Add(new KeyValue("normSInv(数值)", "反标准正态累积分布"));
            temp.Add(new KeyValue("betaDist(数值, 分布参数α, 分布参数β)", "Beta累积分布"));
            temp.Add(new KeyValue("betaInv(数值, 分布参数α, 分布参数β)", "反Beta累积分布"));
            temp.Add(new KeyValue("binomDist(试验成功次数, 试验次数, 成功概率, 返回类型0/1)", "一元二项式分布"));
            temp.Add(new KeyValue("exponDist(函数值, 参数值, 返回类型0/1)", "指数分布"));
            temp.Add(new KeyValue("fDist(数值X, 分子自由度, 分母自由度)", "F概率分布"));
            temp.Add(new KeyValue("fInv(分布概率, 分子自由度, 分母自由度)", "反F概率分布"));
            temp.Add(new KeyValue("fisher(数值)", "Fisher变换"));
            temp.Add(new KeyValue("fisherInv(数值)", "反fisher变换"));
            temp.Add(new KeyValue("gammaDist(数值, 分布参数α, 分布参数β, 返回类型0/1)", "伽玛函数"));
            temp.Add(new KeyValue("gammaInv(分布概率, 分布参数α, 分布参数β)", "反伽玛函数的"));
            temp.Add(new KeyValue("gammaLn(数值)", "伽玛函数的自然对数"));
            temp.Add(new KeyValue("hypgeomDist(样本成功次数, 样本容量, 样本总体成功次数, 样本总体容量)", "超几何分布"));
            temp.Add(new KeyValue("logInv(分布概率, 算法平均数, 标准偏差)", "反对数累积分布"));
            temp.Add(new KeyValue("logNormDist(数值, 算法平均数, 标准偏差)", "反对数正态分布"));
            temp.Add(new KeyValue("negBinomDist(失败次数, 成功极限次数, 成功概率)", "负二项式分布"));
            temp.Add(new KeyValue("poisson(数值, 算法平均数, 返回类型0/1)", "Poisson分布"));
            temp.Add(new KeyValue("tDist(数值, 自由度, 返回类型1/2)", "t分布"));
            temp.Add(new KeyValue("tInv(分布概率, 自由度)", "反t分布"));
            temp.Add(new KeyValue("weibull(数值, 分布参数α, 分布参数β, 返回类型0/1)", "Weibull分布"));
            temp.Add(new KeyValue("lookUp(JSON数组, 公式字符串, 属性名)", "纵向查找函数"));
            temp.Add(new KeyValue("lookUp(JSON数组, 数组, 可选是否区间)", "纵向查找函数"));
            temp.Add(new KeyValue("regexRepalce(文本, 匹配文本, 替换文本)", "正则替换"));
            temp.Add(new KeyValue("isRegex(文本, 匹配文本)", "正则匹配"));
            temp.Add(new KeyValue("isMatch(文本, 匹配文本)", "正则匹配"));
            temp.Add(new KeyValue("trimStart(文本)", "消空文本左边"));
            temp.Add(new KeyValue("lTrim(文本)", "消空文本左边"));
            temp.Add(new KeyValue("trimEnd(文本)", "消空文本右边"));
            temp.Add(new KeyValue("rTrim(文本)", "消空文本右边"));
            temp.Add(new KeyValue("indexOf(文本, 查找文本, 可选开始位置, 可选索引)", "查找文本位置"));
            temp.Add(new KeyValue("lastIndexOf(文本, 查找文本, 可选开始位置, 可选索引)", "查找文本位置"));
            temp.Add(new KeyValue("split(文本, 分隔符)", "分割文本"));
            temp.Add(new KeyValue("join(文本A, 文本B,,,)", "合并文本"));
            temp.Add(new KeyValue("subString(文本, 位置, 可选数量)", "切割文本"));
            temp.Add(new KeyValue("startsWith(文本, 开始文本, 可选忽略大小写1/0)", "匹配开头"));
            temp.Add(new KeyValue("endsWith(文本, 结尾文本, 可选是否忽略大小写1/0)", "匹配结尾"));
            temp.Add(new KeyValue("isNullOrEmpty(文本)", "判断null或空"));
            temp.Add(new KeyValue("isNullOrWhiteSpace(文本)", "判断null、空、空白字符"));
            temp.Add(new KeyValue("removeStart(文本, 左边文本, 可选忽略大小写1/0)", "去除左边匹配文本"));
            temp.Add(new KeyValue("removeEnd(文本, 左边文本, 可选忽略大小写1/0)", "去除右边匹配文本"));
            temp.Add(new KeyValue("json(文本)", "转json格式"));
            temp.Add(new KeyValue("has(文本)", "...包含"));
            temp.Add(new KeyValue("in(集合)", "...在...之中"));
            temp.Add(new KeyValue("error(\"错误信息\")", "抛出错误"));
            temp.Add(new KeyValue("param(文本, 可选默认值)", "获取参数值"));
            temp.Add(new KeyValue("addYears(时间, 数值)", "时间增加年"));
            temp.Add(new KeyValue("addMonths(时间, 数值)", "时间增加月"));
            temp.Add(new KeyValue("addDays(时间, 数值)", "时间增加日"));
            temp.Add(new KeyValue("addHours(时间, 数值)", "时间增加时"));
            temp.Add(new KeyValue("addMinutes(时间, 数值)", "时间增加分"));
            temp.Add(new KeyValue("addSeconds(时间, 数值)", "时间增加秒"));
            #endregion 基础方法

            var helper = GetReadHelper();
            if (appid > 0) {
                //初始值 会替换  输入项 ，所以初始值在前
                var initValues = await helper.Select_Async<DbAppInitValue>("where MainMemberId=@0 and projectId=@1 and AppIds like @2 and IsDelete=0", mainMemberId, projectId, "%," + appid + ",%");
                foreach (var item in initValues) {
                    if (temp.Any(q => q.caption == item.Name) == false) {
                        temp.Add(new KeyValue(item.Name, "初始值"));
                    }
                }
                var inputs = await helper.Select_Async<DbAppInput>("where MainMemberId=@0 and projectId=@1 and AppIds like @2 and IsDelete=0", mainMemberId, projectId, "%," + appid + ",%");
                foreach (var item in inputs) {
                    if (temp.Any(q => q.caption == item.InputName) == false) {
                        temp.Add(new KeyValue(item.InputName, "输入项"));
                    }
                }

                // 项目公式 有具体算法，所以 项目公式在前
                var formulas = await helper.Select_Async<DbProjectFormula>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
                foreach (var item in formulas) {
                    if (temp.Any(q => q.caption == item.Name) == false) {
                        if (item.IsReferenceCode) {
                            temp.Add(new KeyValue(item.Name, item.Formula, "公式"));
                        } else {
                            temp.Add(new KeyValue(item.Name, "公式名称"));
                        }
                    }
                }
            }

            var dicts = await helper.Select_Async<DbProjectDict>("where MainMemberId=@0 and projectId=@1 and IsDelete=0", mainMemberId, projectId);
            foreach (var item in dicts) {
                if (temp.Any(q => q.caption == item.Name) == false) {
                    temp.Add(new KeyValue(item.Name, "项目字典"));
                }
            }
            temp = temp.OrderBy(q => q.caption).ToList();
            string key = "ace_keywords=" + JsonUtil.SerializeObject(temp);
            return key;
        }

        public class KeyValue
        {
            public KeyValue(string key, string meta)
            {
                this.caption = key.Split('(')[0];
                var py = Words.WordsHelper.GetFirstPinyin(caption);
                if (py != caption) {
                    this.pinyin = py;
                }
                this.text = key;
                this.meta = meta;
            }

            public KeyValue(string key, string text, string meta)
            {
                this.caption = key;
                var py = Words.WordsHelper.GetFirstPinyin(caption);
                if (py != caption) {
                    this.pinyin = py;
                }
                this.text = text;
                this.meta = meta;
            }

            [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string pinyin { get; set; }

            public string caption { get; set; }
            public string text { get; set; }

            public string meta { get; set; }
        }

        #endregion GetAceKeywords
    }
}