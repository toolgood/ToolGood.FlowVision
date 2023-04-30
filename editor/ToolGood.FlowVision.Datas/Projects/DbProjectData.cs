using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
    [ReadyGo3.Attributes.Table("flow_project_data")]
    [ReadyGo3.Attributes.Index("MainMemberId")]
    public class DbProjectData
    {
        public int Id { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int MainMemberId { get; set; }

        public int ProjectId { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [FieldLength(64)]
        public string Category { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [FieldLength(100)]
        public string Name { get; set; }

        [FieldLength(500)]
        public string Comment { get; set; }

        [ReadyGo3.Attributes.LongText]
        public string Data { get; set; }

        public DateTime CreateTime { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int CreateUserId { get; set; }

        public DateTime ModifyTime { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int ModifyUserId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsDelete { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? DeleteTime { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int DeleteUserId { get; set; }

        public static DbProjectData CreateBy(Request<DbProjectData> request)
        {
            return new DbProjectData() {
                MainMemberId = request.MainMemberId,
                ProjectId = request.Data.ProjectId,

                Category = request.Data.Category?.Trim(),
                Name = request.Data.Name?.Trim(),
                Data = request.Data.Data?.Trim(),
                Comment = request.Data.Comment?.Trim(),

                CreateTime = DateTime.Now,
                CreateUserId = request.OperatorId,
                ModifyTime = DateTime.Now,
                ModifyUserId = request.OperatorId,
            };
        }

        public void EditBy(Request<DbProjectData> request)
        {
            Category = request.Data.Category.Trim();
            Name = request.Data.Name.Trim();
            Data = request.Data.Data?.Trim();
            Comment = request.Data.Comment?.Trim();


            ModifyTime = DateTime.Now;
            ModifyUserId = request.OperatorId;
        }
    }
}