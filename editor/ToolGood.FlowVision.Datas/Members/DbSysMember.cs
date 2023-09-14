using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.Datas
{
	[Table("Sys_Member")]
	[Index("Name")]
	[Index("MainMemberId")]
	public class DbSysMember
	{
		public int Id { get; set; }

		public int MainMemberId { get; set; }
		public int GroupId { get; set; }

		[ToolGood.ReadyGo3.Attributes.FieldLength(1000)]
		public string ProjectIds { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[UserNameLength]
		public string Name { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[PasswrodLength]
		public string Password { get; set; }

		/// <summary>
		/// 盐值
		/// </summary>
		[PasswrodLength]
		public string Salt { get; set; }

		[ToolGood.ReadyGo3.Attributes.FieldLength(50)]
		public string Department { get; set; }

		[UserNameLength]
		public string TrueName { get; set; }

		[ToolGood.ReadyGo3.Attributes.FieldLength(20)]
		[System.Text.Json.Serialization.JsonIgnore]
		public string JobNo { get; set; }

		/// <summary>
		/// 手机
		/// </summary>
		[PhoneLength]
		public string Phone { get; set; }

		/// <summary>
		/// 邮箱
		/// </summary>
		[EmailLength]
		public string Email { get; set; }

		[ToolGood.ReadyGo3.Attributes.FieldLength(500)]
		public string Comment { get; set; }

		public bool IsFrozen { get; set; }

		public bool IsSystem { get; set; }
		public DateTime CreateTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public bool IsDelete { get; set; }

		public DateTime ModifyTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		public static DbSysMember CreateBy(Request<DbSysMember> request)
		{
			return new DbSysMember() {
				MainMemberId = request.Data.MainMemberId,
				GroupId = request.Data.GroupId,
				ProjectIds = request.Data.ProjectIds,

				Name = request.Data.Name,
				Password = request.Data.Password,
				Salt = request.Data.Salt,
				Department = request.Data.Department,
				TrueName = request.Data.TrueName,
				JobNo = request.Data.JobNo,
				Phone = request.Data.Phone,
				Email = request.Data.Email,
				Comment = request.Data.Comment,

				CreateTime = DateTime.Now,
				ModifyTime = DateTime.Now,
			};
		}

		public void EditBy(Request<DbSysMember> request)
		{
			GroupId = request.Data.GroupId;
			ProjectIds = request.Data.ProjectIds;

			Department = request.Data.Department;
			TrueName = request.Data.TrueName;
			JobNo = request.Data.JobNo;
			Phone = request.Data.Phone;
			Email = request.Data.Email;
			Comment = request.Data.Comment;

			ModifyTime = DateTime.Now;
		}
	}
}