using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas
{
	[Table("Sys_SettingValue")]
	[Index("CategoryName")]
	[Index("SettingName")]
	[Index("Code")]
	public class DbSysSettingValue
	{
		public int Id { get; set; }

		[ShortNameLength]
		public string CategoryName { get; set; }

		[ShortNameLength]
		public string SettingName { get; set; }

		[ShortNameLength]
		public string Code { get; set; }

		[Text]
		public string Value { get; set; }

		[CommentLength]
		public string Comment { get; set; }

		public DateTime CreateTime { get; set; }
		public DateTime ModifyTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public bool IsDelete { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		public static DbSysSettingValue CreateBy(Request<DbSysSettingValue> request)
		{
			return new DbSysSettingValue() {
				CategoryName = request.Data.CategoryName,
				SettingName = request.Data.SettingName,
				Code = request.Data.Code,
				Value = request.Data.Value,
				Comment = request.Data.Comment,

				CreateTime = DateTime.Now,
				ModifyTime = DateTime.Now,
			};
		}

		public void EditBy(Request<DbSysSettingValue> request)
		{
			CategoryName = request.Data.CategoryName;
			SettingName = request.Data.SettingName;
			Code = request.Data.Code;
			Value = request.Data.Value;
			Comment = request.Data.Comment;

			ModifyTime = DateTime.Now;
		}
	}
}