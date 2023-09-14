using System.Text.Json.Serialization;
using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
	[ReadyGo3.Attributes.Table("flow_app_flow")]
	[ReadyGo3.Attributes.Index("MainMemberId", "ProjectId", "AppCode")]
	[ReadyGo3.Attributes.Index("MainMemberId", "AppCode")]
	public class DbAppFlow
	{
		public int Id { get; set; }

		[JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		[FieldLength(64)]
		public string AppCode { get; set; }

		[ReadyGo3.Attributes.LongText]
		public string FlowString { get; set; }

		public DateTime CreateTime { get; set; }

		[JsonIgnore]
		public int CreateUserId { get; set; }

		public DateTime ModifyTime { get; set; }

		[JsonIgnore]
		public int ModifyUserId { get; set; }

		[JsonIgnore]
		public bool IsDelete { get; set; }

		[JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		[JsonIgnore]
		public int DeleteUserId { get; set; }

		public static DbAppFlow CreateBy(Request<DbAppFlow> request)
		{
			return new DbAppFlow() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,
				AppCode = request.Data.AppCode,

				FlowString = request.Data.FlowString?.Trim(),

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbAppFlow> request)
		{
			FlowString = request.Data.FlowString?.Trim();

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}