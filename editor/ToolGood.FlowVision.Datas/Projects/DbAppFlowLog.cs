//using System.Text.Json.Serialization;
//using ToolGood.FlowVision.Commons;
//using ToolGood.ReadyGo3.Attributes;

//namespace ToolGood.FlowVision.Datas.Projects
//{
//    [ReadyGo3.Attributes.Table("flow_app_flow_log")]
//    [ReadyGo3.Attributes.Index("MainMemberId", "ProjectId", "AppCode")]
//    [ReadyGo3.Attributes.Index("MainMemberId", "AppCode")]
//    public class DbAppFlowLog
//    {
//        public int Id { get; set; }

//        [JsonIgnore]
//        public int MainMemberId { get; set; }

//        public int ProjectId { get; set; }

//        [FieldLength(64)]
//        public string AppCode { get; set; }

//        [ReadyGo3.Attributes.LongText]
//        public string FlowString { get; set; }

//        public DateTime CreateTime { get; set; }

//        [JsonIgnore]
//        public int CreateUserId { get; set; }

 
//        public static DbAppFlowLog CreateBy(Request<DbAppFlow> request)
//        {
//            return new DbAppFlowLog() {
//                MainMemberId = request.MainMemberId,
//                ProjectId = request.Data.ProjectId,
//                AppCode = request.Data.AppCode,

//                FlowString = request.Data.FlowString?.Trim(),

//                CreateTime = DateTime.Now,
//                CreateUserId = request.OperatorId,
//            };
//        }

//    }
//}
